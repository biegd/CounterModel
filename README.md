# CounterModel

Einfaches MVVM-Beispiel in <strong>C# mit WPF</strong>. Die Anwendung ist ein <strong>Zähler</strong>, der bei jedem Klick auf einen Button hochzählt. Sie zeigt ein 
vollständiges MVVM-Muster mit `Model`, `ViewModel`und `View`.

<h2>Voraussetzungen</h2>
- Visual Studio 2022 oder neuer
- Neues VPF-Projekt (.NET6 oder .NET8)


<h2>Projektstruktur</h2>

```csharp
CounterApp/
├── Models/
│   └── CounterModel.cs
├── ViewModels/
│   └── CounterViewModel.cs
├── Views/
│   └── MainWindow.xaml
└── MainWindow.xaml.cs
```

<h2>1. Model</h2>

```csharp
namespace CounterApp.Models
{
    public class CounterModel
    {
        public int Count { get; set; }
    }
}
```

<h2>2 ViewModel - CounterViewModel.cs</h2>

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CounterApp.Models;

namespace CounterApp.ViewModels
{
    public class CounterViewModel : INotifyPropertyChanged
    {
        private CounterModel _counter = new();
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count
        {
            get => _counter.Count;
            set
            {
                if (_counter.Count != value)
                {
                    _counter.Count = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand IncrementCommand { get; }

        public CounterViewModel()
        {
            IncrementCommand = new RelayCommand(o => Count++);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter) => _execute(parameter);
    }
}
```

<h2>3. View - MainWindow.xaml</h2>

```csharp
<Window x:Class="CounterApp.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="clr-namespace:CounterApp.ViewModels"
        Title="Counter App" Height="200" Width="300">
    <Window.DataContext>
        <vm:CounterViewModel />
    </Window.DataContext>

    <Grid>
        <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
            <TextBlock Text="{Binding Count}" FontSize="36" HorizontalAlignment="Center" Margin="10"/>
            <Button Content="Zählen" Command="{Binding IncrementCommand}" Width="100" />
        </StackPanel>
    </Grid>
</Window>
```

<h2>4. Code-Behind (leer lassen!) - MainWindow.xaml.cs</h2>

```csharp
namespace CounterApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // ggf. auskommentieren
        }
    }
}
```

<h2>Wichtige Punkte</h2>
- <strong>Trennung von Logik und UI</strong> durch MVVM
- Verwendung von <strong>INotifyPropertyChanged</strong>
- <strong>Datenbindung</strong> (Binding) in XAML
- Verwendung eines <strong>Commands</strong> (Relay Command)
- Keine Logik im Code-Behind - Clean Architecture


<h2>INotifyPropertyChanged</h2> 
<strong>`INotifyPropertyChanged</strong>ist ein <strong>Interface</strong> in .NET, das verwendet wird, um die <strong>Datenbindung</strong> in WPF und
anderen MVVM-Anwendungen zu ermöglichen. Es benachrichtigt die UI, wenn sich eine Eigenschaft im ViewModel geändert hat - damit die Oberfläche
(View) <strong>automatisch aktualisiert</strong> wird.

<h2>Warum ist das wichtig</h2>
In MVVM trennst Du <strong>Logik</strong> und <strong>Darstellung</strong>. Wenn sich z.B. im ViewModel eine Zahl ändert, soll das automatisch in der
UI erscheinen. Das funktioniert nur, wenn die UI benachrichtigt wird - und dafür brauchst du `INotyfyPropertyChanged`.

<h2>Aufbau des Interfaces</h2>

```csharp
public interface INotifyPropertyChanged
{
    event PropertyChangedEventHandler PropertyChanged;
}
```

Du musst:  
1. Das Interface implementieren
2. Die Methode `OnPropertyChanged`schreiben
3. Die Methode aufrufen, wenn sich eine Property ändert

<h2>Beispiel</h2>

```csharp
public class CounterViewModel : INotifyPropertyChanged
{
    private int _count;

    public event PropertyChangedEventHandler PropertyChanged;

    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = value;
                OnPropertyChanged(nameof(Count)); // UI wird benachrichtigt
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

<h2>Ergebnis is der UI</h2>

Wenn du in der View so etwas hast:

```csharp
<TextBlock Text="{Binding Count}" />
```

und in `Count` im ViewModel sich ändert, wird das Textfeld <strong>sofort aktualisiert</strong> , wei 'INotifyPropertyChanged' die Änderung meldest


<h2>Interface</h2>

Stell dir vor, du sagst:
  "Jede Klasse, die `IDruckbar`implementiert, muss eine `Drucken()`-Methode haben"

```csharp
public interface IDruckbar
{
    void Drucken();
}
```

Jetzt kann jede Klasse, die `IDruckbar`implementiert, frei entscheiden <strong>wie</strong> sie druckt:

```csharp
public class Rechnung : IDruckbar
{
    public void Drucken()
    {
        Console.WriteLine("Rechnung wird gedruckt...");
    }
}

public class Etikett : IDruckbar
{
    public void Drucken()
    {
        Console.WriteLine("Etikett wird gedruckt...");
    }
}
```

<h2>Warum sind Interfaces nützlich</h2>

1. <strong>Lose Kopplung:</strong> Der Code bleibt flexibel und testbar
2. <strong>Polymorphismus:</strong> Du kannst verschiedene Objekte <strong>gleich behandeln</strong>, wenn sie dasselbe Interface implementieren.
3. <strong>Austauschbarkeit:</strong> Neue Klassen können leicht hinzugefügt werden, solange sie das Interface einhalten.
4. <strong>MVVM und Events:</strong> Viele .NET-Funktionen (z.B. Datenbindung mit `INotifyPropertyChanged`) basiereen auf `Interfaces`


<h2>Alltagsanalogie</h2>

Ein Interface wie eine <strong>Steckdose:</strong>
- Sie legt fest: So sieht der Anschluss aus
- Du kannst verschiedene Geräte anschließen, solange sie passen
- Was das Gerät tut, bleibt ihm überlassen

<h2>Fazit</h2>
Ein <strong>Interface</strong> ist:

| <strong>Merkmal</strong>              | <strong>Bedeutung</strong>           |
|:--------------------------------------|:-------------------------------------|
| <strong>Vertrag</strong>              | Legt fest, was die Klasse können muss|
| <strong>Keine Implementierung</strong>| Nur Methodensignaturen, keine Logik  |
| <strong>Flexibilität</strong>         | Macht Code flexibel, erweiterbar, <br>testbar|
| <strong>Wichtig für MVVM</strong>     | z.B. `INotifyPropertyChanged`, `ICommand`<br> `IDisposable`|





