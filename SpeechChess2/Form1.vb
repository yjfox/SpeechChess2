Imports SpeechLib
'Copyright - developed for Interactive System Homework - speech program
'Group members - J.Yin, C.Zhou, R.Zhang 

Public Class Form1
    Dim WithEvents RecoContext As SpSharedRecoContext      'The Main Recognition Object Used throughout the whole program. -- Shared Object: More Info on this later.

    Dim Grammar As ISpeechRecoGrammar                      'The Grammar Object so the program knows what is going on. -- Instanced Object: More Info on this later.

    Dim CharCount As Integer                                'This is used to count the amount of chars that are in the text box.




    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click



        'First check to see if reco has been loaded before. If not lets load it.
        If (RecoContext Is Nothing) Then

            RecoContext = New SpSharedRecoContext        'Create a new Reco Context Class

            Grammar = RecoContext.CreateGrammar           'Setup the Grammar

            Grammar.CmdLoadFromFile("c:\sol.xml")           'Load the Grammar

        End If



        lblStatus.Text = "Recognition Started"                'Change the Label to let the user know whats up

        Grammar.CmdSetRuleIdState(0, SpeechRuleState.SGDSActive)   'Turns on the Recognition with my grammar. This is Vitally important



        'This is so the user doesn't break the program by

        'trying to start the recognition after its already started.

        btnStart.Enabled = False

        btnStop.Enabled = True

    End Sub
    'Stop Button. This will stop stop the recoginition

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click



        Grammar.DictationSetState(SpeechRuleState.SGDSInactive) 'Turns off the Recognition. It will go dormant.

        lblStatus.Text = "Recognition Stopped"                'Change the label to let the user know whats up



        'Again This is so the user doesn't go breaking things accidently

        btnStart.Enabled = True

        btnStop.Enabled = False

    End Sub


    'This is the hypothesis sub. The hypothesis is not the final recognition. This will fire many times per word. You do not want to print anything that is final from the hypothesis.

    'This is not required for the final recognition. But it is vital to understand it.

    Private Sub OnHypo(ByVal StreamNumber As Integer, ByVal StreamPosition As Object, ByVal Result As ISpeechRecoResult) Handles RecoContext.Hypothesis



        btnStop.Enabled = False 'Don't allow the user to stop the recognition until it has completed.

        'The button will re-enable in the OnReco Event


        'This is so you don't kepp printing the same text over and over. It could take up just a tiny bit more processor power

        'Its good to not do un-needed things.

        If lblStatus.Text <> "Receiving" Then

            lblStatus.Text = "Receiving"

        End If

    End Sub
    Private Sub OnReco(ByVal StreamNumber As Integer, ByVal StreamPosition As Object, ByVal RecognitionType As SpeechRecognitionType, ByVal Result As ISpeechRecoResult) Handles RecoContext.Recognition

        Dim recoResult As String = Result.PhraseInfo.GetText 'Create a new string, and assign the recognized text to it.

        'If you would prefer to use the SendKeys method, Comment out this entire block. And Uncomment the SendKeys Line.
        txtBox.Selectionstart = CharCount
        If recoResult = "begin" Then

            txtBox.SelectedText = recoResult & Chr(10)
            CharCount = CharCount + 1 + Len(recoResult)
            Call cmdrestart_Click()
            RecoContext.Voice.Speak("now game start,black's turn at first, please put black chess piece down!")
        End If

        Dim wordX(14) As String
        wordX(0) = "one"
        wordX(1) = "two"
        wordX(2) = "three"
        wordX(3) = "four"
        wordX(4) = "five"
        wordX(5) = "six"
        wordX(6) = "seven"
        wordX(7) = "eight"
        wordX(8) = "nine"
        wordX(9) = "ten"
        wordX(10) = "eleven"
        wordX(11) = "twelve"
        wordX(12) = "thirteen"
        wordX(13) = "fourteen"
        wordX(14) = "fifteen"
        Dim wordY(14) As String
        wordY(0) = "one"
        wordY(1) = "two"
        wordY(2) = "three"
        wordY(3) = "four"
        wordY(4) = "five"
        wordY(5) = "six"
        wordY(6) = "seven"
        wordY(7) = "eight"
        wordY(8) = "nine"
        wordY(9) = "ten"
        wordY(10) = "eleven"
        wordY(11) = "twelve"
        wordY(12) = "thirteen"
        wordY(13) = "fourteen"
        wordY(14) = "fifteen"


        Dim wX As Integer
        Dim wY As Integer

        'map word x and y to integer x and y
        For wX = 0 To 14 Step 1
            For wY = 0 To 14 Step 1
                If recoResult = wordX(wX) & " " & wordY(wY) Then
                    txtBox.SelectedText = recoResult & Chr(10)
                    wX = wX + 1
                    wY = wY + 1
                    Me_Draw_VoiceDown(wX, wY)
                End If
            Next wY
        Next wX




        lblStatus.Text = "Finished Dictating"

        btnStop.Enabled = True

    End Sub


    '==================================================================================
    'Speech ends, Five chess begins
    '==================================================================================

    Dim blackturn As Boolean 'black turn 
    Dim whiteturn As Boolean 'white turn
    Dim table(0 To 15, 0 To 15) As Integer 'arrary stores the current status of chess game
    Dim inti As Integer 'array(intx,inty)
    Dim intj As Integer
    Dim intx As Integer 'x coordinate 
    Dim inty As Integer 'y coordinate
    Dim boolstatus As Boolean 'indicate chess status : running or end
    'draw module
    Dim mybitmap As Bitmap
    Dim Graph As Graphics

    'judge if there is a winner
    Private Sub judgeman()

        Dim strwho As String 'potential winner 

        If table(inti, intj) = 1 Then '1=black, otherwise white 
            strwho = "Black"
        Else
            strwho = "White"
        End If


        If samelinenums(1, 0) >= 5 Or samelinenums(0, 1) >= 5 Or samelinenums(1, 1) >= 5 Or samelinenums(-1, 1) >= 5 Then
            PictureBox5.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\nan_win.png")
            MsgBox(strwho & " Win!")
            boolstatus = False 'chess end status flag

        End If
    End Sub

    'detect how many pieces in one line
    Function samelinenums(changei As Integer, changej As Integer)
        Dim i As Integer
        Dim j As Integer
        Dim num As Integer 'how many pieces in one line

        'one side
        i = inti : j = intj
        Do
            If table(i, j) <> table(inti, intj) Then
                num = max(Math.Abs(inti - i), Math.Abs(intj - j))
                Exit Do
            End If
            i = i + changei : j = j + changej
        Loop Until i < 0 Or i > 15 Or j < 0 Or j > 15

        'the other side
        i = inti : j = intj
        Do
            If table(i, j) <> table(inti, intj) Then
                num = num - 1 + max(Math.Abs(inti - i), Math.Abs(intj - j))
                Exit Do
            End If
            i = i - changei : j = j - changej
        Loop Until i < 0 Or i > 15 Or j < 0 Or j > 15
        'MsgBox num 
        samelinenums = num
    End Function

    'biggest number of two 
    Function max(inta As Integer, intb As Integer)
        max = inta
        If max < intb Then max = intb
    End Function

    'draw a chessborad which is 16*16 and length of 20pix
    Private Sub Me_Draw_Paint()

        mybitmap = New Bitmap(Me_Draw.Width, Me_Draw.Height)
        Me_Draw.Image = mybitmap
        Graph = Graphics.FromImage(Me_Draw.Image)

        Dim i As Integer
        'AutoScaleMode = 3  
        For i = 10 To 330 Step 20
            Graph.DrawLine(Pens.Black, 10, i, 330, i)
            Graph.DrawLine(Pens.Black, i, 10, i, 330)
        Next

    End Sub

    Private Sub Me_Draw_PaintCircle(flag As Integer, x As Integer, y As Integer) 'begin at (10,10),draw chessboard which is 16*16,length of 20pix 

        If flag = 1 Then
            'Call Me_Draw_Paint()
            Me_Draw.Image = mybitmap
            Graph = Graphics.FromImage(Me_Draw.Image)
            Graph.FillEllipse(Brushes.Black, x - 5, y - 5, 10, 10)
        Else
            'Call Me_Draw_Paint()
            Me_Draw.Image = mybitmap
            Graph = Graphics.FromImage(Me_Draw.Image)
            Graph.FillEllipse(Brushes.White, x - 5, y - 5, 10, 10)
        End If
    End Sub

    Private Sub cmdrestart_Click()
        PictureBox4.Visible = False

        'reset 
        Dim i = inti
        Dim j = intj

        For i = 0 To 15
            For j = 0 To 15
                table(inti, intj) = 0
            Next
        Next


        blackturn = True 'black first
        boolstatus = True 'game running flag 
        Label1.Text = "Black First"

        'draw the chessboard
        Call Me_Draw_Paint()

    End Sub

    Private Sub cmdrestart_Click(sender As Object, e As EventArgs) Handles cmdrestart.Click
        PictureBox4.Visible = False

        'reset 
        Dim i = inti
        Dim j = intj

        For i = 0 To 15
            For j = 0 To 15
                table(inti, intj) = 0
            Next
        Next


        blackturn = True 'black first
        boolstatus = True 'game running flag 
        Label1.Text = "Black First"
        'draw the chessboard
        Call Me_Draw_Paint()

    End Sub

    Private Sub Me_Draw_VoiceDown(vX As Integer, vY As Integer)
        PictureBox4.Visible = False 'member pic 
        'judge if the game is running
        If boolstatus = False Then
            Label1.Text = "End"
            Exit Sub
        End If



        txtBox.Text = vX & "+" & vY & Chr(10)
        'convert to pixel X/Y axis
        intx = vX * 20 + 10
        inty = vY * 20 + 10
        'transfer coordinate into array "table"
        inti = (intx - 10) / 20
        intj = (inty - 10) / 20

        'if there is alreay a chess piece exist
        If table(inti, intj) <> 0 Then
            Exit Sub
        End If

        'Draw chess pieces
        If blackturn = True Then
            'black

            Me_Draw_PaintCircle(1, intx, inty)
            table(inti, intj) = 1 'black=1 in array
            Label1.Text = "White Turn"
            PictureBox1.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\chaoran.png")
            PictureBox5.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\nan_chess_white.png")

        Else
            'white
            Me_Draw_PaintCircle(2, intx, inty)
            table(inti, intj) = 2 'white=2 in array
            Label1.Text = "Black Turn"
            PictureBox1.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\jun_chess.png")
            PictureBox5.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\nan_chess_black.png")
        End If


        'Circle (intx, inty), 8 

        'judge whether anyone win
        Call judgeman()

        'turn 
        blackturn = Not blackturn

        If blackturn = True Then
            RecoContext.Voice.Speak("it's white's turn now, next is black's turn!")
        Else
            RecoContext.Voice.Speak("it's black's turn now ,next is white's turn!")
        End If
    End Sub

    Private Sub Me_Draw_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me_Draw.MouseDown

        'judge if the game is running
        If boolstatus = False Then
            Label1.Text = "End"
            Exit Sub
        End If


        If e.X < 10 Or e.X > 310 Or e.Y < 10 Or e.Y > 310 Then
            Exit Sub
        End If
        'coordinate 
        If (e.X - 10) Mod 20 < 10 Then
            intx = e.X - (e.X - 10) Mod 20
        Else
            intx = e.X + 20 - (e.X - 10) Mod 20
        End If
        If (e.Y - 10) Mod 20 < 10 Then
            inty = e.Y - (e.Y - 10) Mod 20
        Else
            inty = e.Y + 20 - (e.Y - 10) Mod 20
        End If
        txtBox.Text = intx & "+" & inty
        'transfer coordinate into array "table"
        inti = (intx - 10) / 20
        intj = (inty - 10) / 20

        'if there is alreay a chess piece exist
        If table(inti, intj) <> 0 Then
            Exit Sub
        End If

        'Draw chess pieces
        If blackturn = True Then
            'black

            Me_Draw_PaintCircle(1, intx, inty)
            table(inti, intj) = 1 'black=1 in array
            Label1.Text = "White Turn"
            PictureBox1.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\chaoran.png")
            PictureBox5.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\nan_chess_white.png")
        Else
            'white
            Me_Draw_PaintCircle(2, intx, inty)
            table(inti, intj) = 2 'white=2 in array
            Label1.Text = "Black Turn"
            PictureBox1.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\jun_chess.png")
            PictureBox5.Image = Image.FromFile("C:\Users\JunY\Documents\Visual Studio 2012\Projects\SpeechChess2\nan_chess_black.png")
        End If


        'Circle (intx, inty), 8 

        'judge whether anyone win
        Call judgeman()

        'turn 
        blackturn = Not blackturn
    End Sub


    Private Sub Cmd_Close_Click(sender As Object, e As EventArgs) Handles Cmd_Close.Click
        Application.Exit()
    End Sub

    Private Sub cmdclose_Click(sender As Object, e As EventArgs) Handles cmdclose.Click
        Application.Restart()
    End Sub




    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

