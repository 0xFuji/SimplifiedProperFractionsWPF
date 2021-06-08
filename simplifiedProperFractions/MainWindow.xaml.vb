Class MainWindow

    ' Notes:
    ' The Await Task.Run is calling slow methods because without it, the GUI would be frozen until the method is finished...
    ' Dispatcher.Invoke simply allows you to interact with controls from the main thread externally.
    ' This math was way too anoying to figure out.

    Private Sub Border_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ' Simply allows you to drag the window from the custom taskbar
        DragMove()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        ' Best practice exit for .NET Framework apps
        Environment.Exit(0)
    End Sub

    Private Async Function simplifiedProperFractions(n) As Task(Of Integer)
        Await Task.Run(Sub() Dispatcher.Invoke(Sub() pgrFractions.Inlines.Clear())) ' Reset rich text box
        Dim total As Integer = 0
        For denom As Integer = 2 To n ' For all denominators less than or equal to the submitted denominator
            For numer As Integer = 1 To denom - 1 ' For all numerators less than current
                If Not commonFactors(denom, numer) Then ' Check for common factors
                    Await Task.Run(Sub() Dispatcher.Invoke(Sub() pgrFractions.Inlines.Add(New Run($"{numer}/{denom} ")))) ' Add fraction to rich text box (Async)
                    total += 1
                    Await Task.Run(Sub() Dispatcher.Invoke(Sub() lblTotal.Content = total)) ' Update total label (Async)
                End If
            Next numer
        Next denom
        Return total
    End Function

    Private Function commonFactors(x, y) As Boolean
        ' Just checks for common factors
        For a As Integer = 2 To x - 1
            If x Mod a = 0 And y Mod a = 0 Then
                Return True
            End If
        Next a
        Return False
    End Function

    Private Async Sub btnSubmit_Click(sender As Object, e As RoutedEventArgs) Handles btnSubmit.Click
        ' Handles button submit for the denominator
        lblTotal.Content = Await Task.Run(Function() simplifiedProperFractions(Dispatcher.Invoke(Function() sldDenom.Value))) ' Async to reduce GUI stuttering, better UX & performance
    End Sub
End Class
