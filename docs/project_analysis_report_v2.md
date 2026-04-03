# プロジェクト分析レポート v4 (修正版)

## 1. アーキテクチャ概要

このプロジェクトは、プラットフォーム固有のUI層と共有コアロジック層が明確に分離された、優れたクロスプラットフォーム設計を採用しています。全体の依存関係は以下の図で示されます。

```mermaid
graph TD
    subgraph "UI Layer (Presentation)"
        A[MidiPlayer.Droid] --> D
        B[MidiPlayer.Win64] --> D
    end

    subgraph "Core Layer (Shared Logic)"
        D[MidiPlayer.FluidSynth] --> C
        C[MidiPlayer] --> E
        D --> E
        D --> F
    end

    subgraph "Data & External Libs"
        E[MidiPlayer.Midi] --> I[Sanford.Multimedia.Midi.dll]
        F[MidiPlayer.SoundFont]
    end

    subgraph "Native Dependencies"
        D --> G["Native FluidSynth (.dll, .so)"]
    end

    style A fill:#D6EAF8
    style B fill:#D6EAF8
    style C fill:#D5F5E3
    style D fill:#D5F5E3
```

- **UI層:** `MidiPlayer.Droid` (Xamarin.Android) と `MidiPlayer.Win64` (WinForms) が各プラットフォームのUIとライフサイクルを管理します。
- **コア層:** `MidiPlayer.FluidSynth` と `MidiPlayer` が中核を担います。オーディオ処理、再生ロジック、UIとの通信など、ビジネスロジックの大部分がここに集約されています。
- **データ層:** `MidiPlayer.Midi` と `MidiPlayer.SoundFont` が、それぞれのファイル形式の解析とデータ抽出を担当します。


## 2. 中核的な通信メカニズム: EventQueue

UIからのコマンドは `EventQueue` を通じて非同期にバックエンドへ送信されます。これにより、UIの応答性が保たれます。

### 2.1. クラス構造

`EventQueue` は、MIDIの16トラックに対応する16個のキューを持つ静的クラスです。UIから送信されるコマンドは `Data` クラスのインスタンスとしてキューに格納されます。

```mermaid
classDiagram
    class EventQueue {
        <<static>>
        - Map<int, Queue<Data>> _queue_map
        + Enqueue(track_index, value) void
        + Dequeue(track_index) Data
    }
    class Data {
        + int Channel
        + int Program
        + int Pan
        + int Volume
        + bool Mute
    }
    EventQueue ..> Data : uses
```

### 2.2. UIからSynthへのシーケンス

ユーザー操作がシンセサイザーに反映されるまでの流れは以下の通りです。この例ではAndroid UIを使用します。

```mermaid
sequenceDiagram
    participant User as "ユーザー"
    participant AndroidUI as "MainActivity"
    participant Mixer
    participant EventQueue
    participant Synth
    participant FluidSynth as "ネイティブFluidSynth"

    User->>AndroidUI: "音量スライダーを操作"
    AndroidUI->>Mixer: "GetCurrent().Volume = value"
    Note right of Mixer: "この時点ではUIの状態(ViewModel)が<br/>更新されるだけで、Synthには送られない"

    User->>AndroidUI: "「Send to Synth」ボタンをクリック"
    AndroidUI->>Mixer: "GetCurrent() を呼び出しFader取得"
    Mixer-->>AndroidUI: "現在のFaderオブジェクト"
    AndroidUI->>EventQueue: "Enqueue(fader.Index, new Data(...))"
    Note right of EventQueue: "コマンドがキューに積まれる"

    Note over Synth: "MIDI再生コールバックが実行中..."
    Synth->>EventQueue: "Dequeue(track_index)"
    EventQueue-->>Synth: "Dataオブジェクト（コマンド）"
    Synth->>FluidSynth: "fluid_synth_cc(..., data.Volume)"
    Note left of FluidSynth: "ネイティブ関数を呼び出し<br/>リアルタイムに音量を変更"
```

## 3. 状態管理とデータモデル

### 3.1. Mixer: UIの状態を保持するViewModel

`Mixer` クラスは、UIに表示される各トラックのパラメータ（音量、パン、楽器など）を管理する静的クラスです。これは実質的にUIのための **ViewModel** として機能し、UIの状態とシンセサイザーの内部状態を分離する重要な役割を担っています。

```mermaid
classDiagram
    class Mixer {
        <<static>>
        - Map<int, Fader> _mixer
        - int _current
        + int Current get/set
        + GetCurrent() Fader
        + event PropertyChangedEventHandler Selected
        + event PropertyChangedEventHandler Updated
    }
    class Fader {
        + int Index
        + bool Sounds
        + string Name
        + int Channel
        + int Program
        + int Volume
        + int Pan
        + event PropertyChangedEventHandler Updated
    }
    Mixer "1" o-- "16" Fader
```
- ユーザーがUIでトラックを選択すると `Mixer.Current` が設定されます。
- スライダーなどを操作すると、対応する `Fader` オブジェクトのプロパティが更新されます。
- これらの変更は `Mixer.Updated` イベントを発行し、必要に応じてUIの他の部分を更新できますが、この時点ではまだバックエンドには送信されません。

## 4. オーディオエンジン: Synth.cs の心臓部

`Synth.cs` は `FluidSynth` ライブラリをラップし、再生の全責任を負います。最も重要なのは、`fluid_player_set_playback_callback` で登録されるリアルタイムコールバックです。

### 4.1. 再生コールバックの処理フロー

このコールバックはMIDIイベントが発生するたびに呼び出され、2つの重要な処理を同時に行います。
1.  MIDIファイル自体のイベント（ノートオン、プログラムチェンジ等）を解釈し、UI表示用のデータモデル `Multi.Track` を更新する。
2.  `EventQueue` をチェックし、UIからの未処理のコマンドがあればそれを取得し、`FluidSynth` に適用する。

```mermaid
graph TD
    A["再生コールバックがMIDIイベントと共にトリガー"] --> B{"イベントタイプを判別"};
    B -->|"ノートオン/オフ"| C["Multi.Track.Sounds を更新しUIへ通知"];
    B -->|"プログラムチェンジ"| D["Multi.Track.Program を更新しUIへ通知"];
    B -->|"コントロールチェンジ"| E["Multi.Track.Bank/Volume/Pan を更新しUIへ通知"];
    C --> F["全16トラックをループ"];
    D --> F;
    E --> F;
    F --> G{"EventQueue.Dequeue(当該トラック)"};
    G -->|"コマンド(Data)有り"| H["fluid_synth_... を呼び出しシンセに適用"];
    H --> F;
    G -->|"コマンド無し"| F;
    F --"ループ終了"--> I["fluid_synth_handle_midi_event を呼び出し終了"];
```
この設計により、MIDIファイルからのイベント処理と、UIからのリアルタイムなパラメータ変更が、単一の低遅延な処理ループ内で効率的に統合されています。

## 5. 結論: 2倍詳細な分析からの洞察

- **明確な責務分離:** UIの状態（`Mixer`）、UIとコアの通信（`EventQueue`）、オーディオエンジンの状態（`Synth.Multi`）が明確に分離されており、それぞれが独立して機能します。これは非常に洗練された設計です。
- **2段階の更新パターン:** Android UIの操作は、まず `Mixer` (ViewModel) を更新し、次に「Send to Synth」ボタンによって明示的に `EventQueue` にコマンドを送信します。これにより、スライダー操作中に大量のイベントが発行されるのを防ぎ、ユーザーの意図した最終的な値のみを効率的に送信できます。
- **リアルタイム性と応答性の両立:** `EventQueue` を介した非同期コマンド送信と、再生コールバック内でのコマンド消費というパターンは、UIの応答性を損なうことなく、低遅延でのオーディオパラメータ変更を実現するための効果的な解決策です。
- **プラットフォーム間の機能差:** Windows版UIが `EventQueue` への書き込みを行わない「閲覧専用」クライアントである一方、Android版が完全な編集機能を持つことから、このアプリケーションが主にAndroid向けに開発された、あるいはAndroid版がより成熟したバージョンであることが強く示唆されます。
