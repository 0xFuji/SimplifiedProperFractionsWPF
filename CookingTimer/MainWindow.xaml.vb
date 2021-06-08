Imports System.Windows.Threading
Imports System.Text.RegularExpressions
Imports Humanizer
Imports System.Media

Class MainWindow
    Private dpTimer As DispatcherTimer = New DispatcherTimer ' Timer
    Private secondsLeft As Integer = 0 ' Seconds Left on timer.
    Private r As Regex = New Regex("[^0-9]+") ' Regex for numbers 0-9 for the text inputs.

    Public Sub New() ' Sub run on launch.
        InitializeComponent()
        secondsLeft = (Convert.ToInt32(txtMinutes.Text) * 60) + Convert.ToInt32(txtSeconds.Text) ' Sets secondsLeft on launch
    End Sub

    Public Sub startTimer() ' Starts the timer.
        If Not dpTimer.IsEnabled Then ' Checks that timer is not enabled, otherwise if you spam start it just ads more timers, doubling the speed of ticks.
            ' Disabling and enabling objects (Just for UX, no functional effect.)
            btnStop.IsEnabled = True
            btnStart.IsEnabled = False
            txtMinutes.IsEnabled = False
            txtSeconds.IsEnabled = False
            dpTimer.Interval = TimeSpan.FromSeconds(1) ' Sets timer interval to tick each second
            AddHandler dpTimer.Tick, AddressOf Tick ' Adds tick handler
            dpTimer.Start() ' Starts timer.
        End If
    End Sub

    Public Sub stopTimer() Handles btnStop.Click
        ' Disabling and enabling objects (Just for UX, no functional effect.)
        btnStop.IsEnabled = False
        btnStart.IsEnabled = True
        txtMinutes.IsEnabled = True
        txtSeconds.IsEnabled = True
        dpTimer.Stop() ' Stops timer
        dpTimer = New DispatcherTimer ' Resets timer, otherwise they would double up.
    End Sub
    Public Sub resetTimer()
        dpTimer.Stop() ' Stops timer
        dpTimer = New DispatcherTimer ' Resets timer, otherwise they would double up.
        secondsLeft = (Convert.ToInt32(txtMinutes.Text) * 60) + Convert.ToInt32(txtSeconds.Text) ' Resets secondsLeft
        ' Disabling and enabling objects (Just for UX, no functional effect.)
        btnStop.IsEnabled = False
        btnStart.IsEnabled = True
        txtMinutes.IsEnabled = True
        txtSeconds.IsEnabled = True
        lblTime.Content = TimeSpan.FromSeconds(secondsLeft).Humanize(4) ' Sets label to humanized time.
    End Sub

    Private Sub Tick()
        If secondsLeft > 0 Then ' Checks if timer is still going.
            secondsLeft -= 1
            lblTime.Content = TimeSpan.FromSeconds(secondsLeft).Humanize(4) ' Sets label to humanized time.
        Else
            SystemSounds.Beep.Play() ' Plays system beep, easiest way to alert user.
        End If
    End Sub

    Private Async Sub NumberValidationTextBox(sender As Object, e As TextCompositionEventArgs) Handles txtMinutes.PreviewTextInput, txtSeconds.PreviewTextInput
        e.Handled = r.IsMatch(e.Text) ' Checks regex
        Await Task.Run(Sub()
                           Threading.Thread.Sleep(50) ' Waits 50 milleseconds on seperate thread for text to update and to make sure the UI thread is uneffected.
                           Dispatcher.Invoke(Sub() secondsLeft = (Convert.ToInt32(txtMinutes.Text) * 60) + Convert.ToInt32(txtSeconds.Text)) ' Sets secondsLeft on main thread.
                       End Sub)
    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        ' Best practice exit for .NET Framework apps
        Environment.Exit(0)
    End Sub

    Private Sub bdrHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles bdrHeader.MouseLeftButtonDown
        ' Simply allows you to drag the window from the custom taskbar
        DragMove()
    End Sub
End Class
