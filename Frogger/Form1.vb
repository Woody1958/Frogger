
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Public Class Form1
    Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uRetrunLength As Integer, ByVal hwndCallback As Integer) As Integer

    'general game variables
    Dim FrogLives As Integer = 5
    Dim IsRunning As Boolean = True
    'game timing variables
    Dim sw As New Stopwatch
    Dim tSec As Integer = 0
    Dim frames As Integer = 0
    Dim FPS As Integer = 0
    Dim ET As Double = 0
    Dim PT As Double = 0
    'map variables
    Dim NumOfMaps As Integer = 0    'number of maps
    Dim NumOfRows As Integer = 0    'number of lanes
    Dim NumofSprites As Integer = 0 'lanelength
    Dim GameMaps(,,) As Single 'map, rows(lanes), columns(lanelength)
    Dim LaneVelocity(10, 13) As Single
    'round variables
    Dim NewRoundFlag As Boolean = True
    Dim CurrentRound As Integer = -1
    Dim CurrentMap As Integer = 0
    Dim ShowMap As Integer = 0
    Dim ShownLaneLength As Integer = 32
    Dim StartTrigger As Boolean = False
    Dim StartTrigger2 As Boolean = False
    'drawing and animation variables
    Dim Sprites(70) As Bitmap
    Dim TurtleSeq(2) As Integer
    Dim SinkingTurtleSeq(7) As Integer
    Dim AlligatorSeq(1) As Integer
    Dim FrogRoadDeadSeq(4) As Integer
    Dim FrogWaterDeadSeq(4) As Integer
    Dim DeadFrogTime As Double
    Dim DeadFrogWater As Boolean = False
    Dim TimePics(7) As Bitmap


    Dim Home(4) As Integer
    Dim HomeRect(4) As Rectangle
    Dim TimeLeft As Single = 60
    Dim StartClock As Boolean = True
    Dim StatusBarPics(1) As Bitmap

    Dim CRect(31, 12) As Rectangle
    Dim FrogSprite As Integer = 0
    Dim FRect As Rectangle
    Dim LFRect As Rectangle
    Dim GotLady As Boolean = false
    Dim FrogX As Single = 224
    Dim FrogY As Integer = 449
    Dim frogVelocity As Single = 0
    Dim FrogLane As Integer = 12
    Dim FrogDead As Boolean = False
    Dim TimeFlag As Integer = 0
    Dim HomePopUpTime As Single
    Dim RandomHome As Integer = 0
    Dim ClearHomePopUpTime As Single
    Dim FrogHome As Boolean = False
    Dim HomeTimerStart As Boolean = False
    Dim HomeTimer As Integer = 0

    Dim Score As Integer = 0
    Dim HiScore As Integer = 4630
    Dim TimeLeftScore As Integer
    Dim TimeLeftFlag As Single
    Dim MaxLane As Integer = 12
    Dim ladyFrogFlag As Single
    Dim LadyFrogSprite As Integer = -2

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Show()
        Me.Focus()
        Me.ClientSize = New System.Drawing.Size(448, 512)
        'get the sprite bitmaps
        LoadSprites()
        ClearHome()
        GetMaps()
        LoadAnimationSequence()
        'start game
        sw.Restart()
        GameLoop()
    End Sub

    Private Sub GameLoop()
        NewRound()
        ResetRound()
        Dim Path As String = Application.StartupPath
        Path = Chr(34) & Path & "\Background.mp3" & Chr(34)
        mciSendString("open " & Path & " alias " & "BackGround", Nothing, 0, 0)
        mciSendString("setaudio BackGround volume to 100", Nothing, 0, 0)
        mciSendString("play BackGround repeat", Nothing, 0, 0)
        Do While IsRunning = True
            'keep alive
            Application.DoEvents()

            'update game time and get FPS
            tickCounter()

            'get initial map and update on the fly
            GetNextMap()


            'do graphics
            drawGame()

            'check to see if the frog is dead
            CheckIfFrogDead()

            'check if frog picked up lady frog
            If Not FrogDead Then
                If FRect.IntersectsWith(LFRect) Then
                    GotLady = True
                    LadyFrogSprite = -2
                End If
            End If

            'if frog dead do the animation sequence
            If FrogDead Then DoDeadFrogAnimation()

            'check to see if round cleared
            CheckIfRoundCleared()

            'check if time to popup a bug or croc at home
            CheckIfHomePopUpTime()

            'get points if frg advances on screen
            If Not FrogDead Then
                If FrogLane < MaxLane And FrogLane <> 0 And FrogLane <> 6 Then
                    MaxLane = FrogLane
                    Score += 10
                End If
            End If

            If Score > HiScore Then HiScore = Score
            'If NewRoundFlag And FrogLane <= 11 Then StartTrigger = True
        Loop
        mciSendString("close BackGround", CStr(0), 0, 0)

        If MsgBox("would you like to play again?", vbYesNo) = vbNo Then End
        CurrentRound = -1
        Score = 0
        FrogLives = 5
        ClearHome()
        IsRunning = True
        GameLoop()
    End Sub
    Private Sub drawGame()
        Dim GameImage As New Bitmap(My.Resources.Largefroggerbackground)
        Dim g As Graphics
        Dim StartPos As Integer = 0
        Dim cellOffset As Integer = 0
        'collision rectangles
        Array.Clear(CRect, 0, CRect.Length)
        'set background
        g = Graphics.FromImage(GameImage)

        'draw time
        Dim toffset As Integer = 0  'offset for color
        If Int(TimeLeft) > 0 Then
            If TimeLeft <= 12 Then toffset = 4 'change to red color if time is low
            If TimeLeft <= 12 Then TimeFlag += 1
            'If TimeFlag = 1 Then My.Computer.Audio.Play(My.Resources.sound_frogger_time, AudioPlayMode.Background)
            If TimeFlag = 1 Then
                Dim Path As String = Chr(34) & "C:\Users\roger\Desktop\Projects\Frogger\Video\sound-frogger-time.wav" & Chr(34)
                mciSendString("open " & Path & " alias " & "Timer", CStr(0), 0, 0)
                'mciSendString("setaudio Timer volume to 1000", CStr(0), 0, 0)
                mciSendString("play Timer", CStr(0), 0, 0)
            End If
            For i = 0 To Int((TimeLeft - 1) / 4) - 1
                g.DrawImage(TimePics(0 + toffset), 364 - (i * 16), 495)
            Next
            g.DrawImage(TimePics(3 - (Int(TimeLeft - 1) Mod 4) + toffset), 364 - (Int((TimeLeft - 1) / 4) * 16), 495)
        End If
        'draw froglives
        For i = 1 To FrogLives - 1
            g.DrawImage(StatusBarPics(0), (i - 1) * 16, (13 * 32) + 65)
        Next i
        'draw current round bug
        For i = 0 To CurrentRound
            g.DrawImage(StatusBarPics(1), 400 - (i - 1) * 16, (13 * 32) + 65)
        Next i
        'draw home
        For i = 0 To 4
            If Home(i) <> -1 Then g.DrawImage(Sprites(Home(i)), (i * 3 * 32) + 16, 65)
        Next i

        'draw moving logs And cars
        Dim laneLength As Integer = NumofSprites
        For y = 0 To 12
            'determine the smoothing offset to draw for this lane
            StartPos = (Fix(PT / 1000 * LaneVelocity(ShowMap, y)) Mod ShownLaneLength)
            cellOffset = Fix(16 * PT / 1000 * LaneVelocity(ShowMap, y)) Mod 16   ' 16 is cell width
            If StartPos < 0 Then StartPos = ShownLaneLength - (Math.Abs(StartPos) Mod ShownLaneLength)
            'End If
            'determine the main calculations for the animation only once for this line
            Dim SinkingTurtleAnimationOffset As Integer = Fix(PT / 1000 * 3) Mod 8
            Dim SinkingTurtleAnimationOffset2 As Integer = Fix(PT / 1000 * 1.8) Mod 8
            Dim TurtleAnimationOffset As Integer = Fix(PT / 1000 * 3) Mod 3
            Dim AlligatorAnimationOffset As Integer = Fix(PT / 1000 * 0.6) Mod 2

            For x = 0 To 30
                'Dim SpriteNum = GameMaps(CurrentMap, y, (StartPos + x) Mod laneLength)
                Dim SpriteNum = GameMaps(ShowMap, y, (StartPos + x) Mod ShownLaneLength)

                'get the animated sprite 
                Select Case SpriteNum
                    Case = 42, 43
                        'alligator
                        SpriteNum = AlligatorSeq(AlligatorAnimationOffset)
                    Case = 47, 48, 49
                        'sinking turtle
                        'lane 2 turtles sink slow  lane 5 turtles sink fast
                        If y = 2 Then
                            SpriteNum = SinkingTurtleSeq(SinkingTurtleAnimationOffset2)
                            If SinkingTurtleAnimationOffset2 < 3 Then SpriteNum = SinkingTurtleSeq(TurtleAnimationOffset)
                        Else
                            SpriteNum = SinkingTurtleSeq(SinkingTurtleAnimationOffset)
                            If SinkingTurtleAnimationOffset < 3 Then SpriteNum = SinkingTurtleSeq(TurtleAnimationOffset)
                        End If
                    Case = 44, 45, 46
                        'swimming turtle
                        SpriteNum = TurtleSeq(TurtleAnimationOffset)
                    Case = 49, 50
                        'otter
                    Case = 51, 52, 53, 54, 55, 56
                        'snake
                    Case Else

                End Select
                If SpriteNum > -1 Then
                    'draw the sprite
                    g.DrawImage(Sprites(SpriteNum), ((x - 2) * 16) - cellOffset, 65 + (y * 32))
                    'create collision rectangle for road sprite
                    Select Case SpriteNum
                        Case 31
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 1, 65 + (y * 32) + 5, 31, 21)
                        Case 32
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 0, 65 + (y * 32) + 1, 31, 29)
                        Case 33
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 0, 65 + (y * 32) + 1, 31, 29)
                        Case 34
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 1, 65 + (y * 32) + 4, 29, 23)
                        Case 35
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 5, 65 + (y * 32) + 5, 29, 20)
                        Case 36
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 2, 65 + (y * 32) + 5, 26, 20)
                        Case 42
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 2, 65 + (y * 32) + 5, 34, 20)
                        Case 43
                            CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 2, 65 + (y * 32) + 5, 34, 20)
                    End Select
                Else
                    If SpriteNum = -1 Then
                        'create collision rectangles for the water and home
                        Select Case y
                            Case = 0
                                'home rectangles
                                Select Case x
                                    Case 1
                                        CRect(x, y) = New Rectangle(((x - 1) * 32) - cellOffset - 27, 65 + (y * 32), 32, 32)
                                    Case 3, 6, 9, 12
                                        CRect(x, y) = New Rectangle(((x - 1) * 32) - cellOffset - 6, 65 + (y * 32), 43, 32)
                                    Case 14
                                        CRect(x, y) = New Rectangle(((x - 1) * 32) - cellOffset + 28, 65 + (y * 32), 32, 32)

                                End Select
                            Case 1 To 5
                                'water rectangles
                                CRect(x, y) = New Rectangle(((x - 2) * 16) - cellOffset + 4, 65 + (y * 32), 4, 32)

                        End Select
                    End If
                End If

                'Dim blackPen = New Pen(Color.FromArgb(255, 255, 255, 255), 1)
                'g.DrawRectangle(blackPen, CRect(x, y))
                'blackPen.Dispose()

            Next
        Next
        'draw scores
        Dim f As New Font("Microsoft Sans Serif", 18, FontStyle.Bold)
        Dim br As New SolidBrush(Color.Red)
        Dim brect As Rectangle
        Dim pt1 As New Point(54, 14)
        Dim sc As String = Score.ToString("D5")
        'score
        g.DrawString(sc, f, br, pt1)
        pt1 = New Point(182, 14)
        sc = HiScore.ToString("D5")
        g.DrawString(sc, f, br, pt1)
        'hi score
        pt1 = New Point(182, 14)
        sc = HiScore.ToString("D5")
        g.DrawString(sc, f, br, pt1)
        'hi score
        If TimeLeftFlag > 0 Then
            brect = New Rectangle(160, 260, 129, 24)
            br = New SolidBrush(Color.Black)
            g.FillRectangle(br, brect)
            br = New SolidBrush(Color.Red)

            pt1 = New Point(168, 258)
            sc = "TIME  " & TimeLeftScore.ToString("D2")
            g.DrawString(sc, f, br, pt1)

        End If
        br.Dispose()
        f.Dispose()
        pt1 = Nothing
        'brect = Nothing
        sc = Nothing
        'draw frog
        If FrogDead Then
            g.DrawImage(Sprites(FrogSprite), Int(FrogX), FrogY)
        Else
            FrogX -= (16 * ET / 1000 * frogVelocity)
            g.DrawImage(Sprites(FrogSprite), Int(FrogX), FrogY)
            FRect = New Rectangle(Int(FrogX) + 4, FrogY + 4, 24, 24)
        End If

        'draw lady frog
        ' lady Frog frog alone
        If Not GotLady Then
            StartPos = (Fix(PT / 1000 * LaneVelocity(ShowMap, 4)) Mod 64)
            cellOffset = Fix(16 * PT / 1000 * LaneVelocity(ShowMap, 4)) Mod 16   ' 16 is cell width
            If StartPos < 0 Then StartPos = 64 - (Math.Abs(StartPos) Mod 64)
            If StartPos = 61 Then LadyFrogSprite = 14
            Label1.Text = StartPos
            For x = 0 To 30
                If (StartPos + x) Mod 64 = 27 Then
                    If LadyFrogSprite > 0 Then
                        g.DrawImage(Sprites(LadyFrogSprite), ((x - 2) * 16) - cellOffset, 65 + (4 * 32))
                        LFRect = New Rectangle(((x - 2) * 16) - cellOffset + 8, 65 + (4 * 32) + 4, 16, 24)
                    End If
                End If
            Next x
        End If
        'lady frog attached
        If GotLady Then
            g.DrawImage(Sprites(FrogSprite + 8), Int(FrogX) - 2, FrogY + 2)
            LFRect = Nothing
        End If



        'Dim blackPen2 = New Pen(Color.FromArgb(255, 255, 255, 255), 1)
        'g.DrawRectangle(blackPen2, LFRect)
        'blackPen2.Dispose()




        Invalidate()
        '***********
        Me.BackgroundImage = GameImage
        GameImage = Nothing
        g.Dispose()
        'display fps
        lblFPS.Text = FPS
        'Label1.Text = ladyFrogFlag
    End Sub
    Private Sub CheckIfFrogDead()
        If FrogDead Then Return
        'check if time is up
        If TimeLeft <= 0 Then
            FrogDead = True
            DeadFrogTime = sw.ElapsedMilliseconds
            StartClock = False
            My.Computer.Audio.Play(My.Resources.RoadDead, AudioPlayMode.Background)
            Return
        End If
        'check that frog is not off screen
        If FrogX <= -32 Or FrogX >= 448 Then
            'frog off screen
            FrogDead = True
            DeadFrogTime = sw.ElapsedMilliseconds
            StartClock = False
            My.Computer.Audio.Play(My.Resources.RoadDead, AudioPlayMode.Background)
            Return
        End If
        'check for collisions on gameboard
        For i = 0 To 30
            If FRect.IntersectsWith(CRect(i, FrogLane)) Then
                If FrogLane > 0 And FrogLane < 6 Then
                    'water
                    DeadFrogWater = True
                    My.Computer.Audio.Play(My.Resources.WaterDead, AudioPlayMode.Background)
                Else
                    'road
                    My.Computer.Audio.Play(My.Resources.RoadDead, AudioPlayMode.Background)
                End If
                FrogDead = True
                DeadFrogTime = sw.ElapsedMilliseconds
                StartClock = False
                Return
            End If
        Next
        'check for collisions in home with gator or existing frog
        If FrogLane = 0 Then
            If (Home(Int(FrogX / 32 / 3)) = 57 Or Home(Int(FrogX / 32 / 3)) = 61) Then
                ' Label1.Text = Int(FrogX / 32 / 3) & " ," & FrogLane
                FrogDead = True
                DeadFrogTime = sw.ElapsedMilliseconds
                StartClock = False
                My.Computer.Audio.Play(My.Resources.RoadDead, AudioPlayMode.Background)
                Return
            Else
                'put big frog in home
                FrogHome = True
                StartClock = False
                Return
            End If
        End If
    End Sub
    Private Sub CheckIfRoundCleared()
        'Label1.Text = HomeTimer
        Dim HomeFilled As Boolean
        If HomeTimerStart Then

            If HomeTimer > 500 Then
                HomeTimer = 0
                HomeTimerStart = False
                ClearHome()
                ResetRound()
                Return
            End If
            Return
        End If

        'Return
        'End If
        If FrogHome Then
            'put big frog in home
            If Home(Int(FrogX / 32 / 3)) = 59 Then
                Score += 200
                'show 200 pts on screen ?
            End If
            Home(Int(FrogX / 32 / 3)) = 57
            'hide little frog
            'FrogX = 224
            'FrogY = 449
            'make home sound
            My.Computer.Audio.Play(My.Resources.GotHome, AudioPlayMode.Background)
            'reset frog variables
            frogVelocity = 0
            'add points
            Score += 50
            'do time score
            TimeLeftScore = Int(TimeLeft)
            Score += (10 * TimeLeftScore)
            TimeLeftFlag = 4000
            'do lady at home score
            If GotLady Then
                Score += 200
                'LadyFrogSprite = -2
                GotLady = False
            End If
            'check if all homes filled
            HomeFilled = True
            For i = 0 To 4
                If Home(i) <> 57 Then
                    HomeFilled = False
                    Exit For
                End If
            Next
            If HomeFilled Then
                HomeTimerStart = True

                NewRound()
                My.Computer.Audio.Play(My.Resources.AllFrogsHome, AudioPlayMode.Background)
                'delay
                FrogLane = 12
                'FrogX = 224
                FrogY = 500 'hide frog
                Score += 1000
                MaxLane = 12
            Else
                ResetRound()
            End If

            'TimeLeft = 60
            'TimeFlag = 0
            'StartClock = True
            'FrogX = 224
            'FrogY = 449
            'FrogLane = 12
            'frogVelocity = 0
            'FrogDead = False
            'DeadFrogWater = False
            FrogHome = False

        End If
    End Sub
    Private Sub GetNextMap()
        If NewRoundFlag Then
            ShowMap = (CurrentRound Mod 10) Mod 5
            ShownLaneLength = 32
        End If
        If StartTrigger Then
            Dim TriggerPoint As Integer = Fix(PT / 1000 * LaneVelocity(ShowMap, 1)) Mod 64
            Dim Triggered As Boolean = False
            Select Case CurrentRound Mod 10
                Case = 0 'no gators no trigger point
                Case = 1, 2, 3
                    If TriggerPoint = -60 Then Triggered = True
                Case Else
                    If TriggerPoint = -60 Then
                        Triggered = True
                        StartTrigger2 = True
                    End If
            End Select
            If Triggered Then
                NewRoundFlag = False
                StartTrigger = False
                ShownLaneLength = 64
                ShowMap = (CurrentRound Mod 10) Mod 5
            End If
        End If
        If StartTrigger2 Then
            Dim TriggerPoint As Integer = Fix(PT / 1000 * LaneVelocity(ShowMap, 1)) Mod 64
            If TriggerPoint = -28 Then
                Select Case CurrentRound Mod 10
                    Case = 4, 9
                        ShowMap = 9
                    Case = 5
                        ShowMap = 5
                    Case = 6
                        ShowMap = 6
                    Case = 7
                        ShowMap = 7
                    Case = 8
                        ShowMap = 8
                End Select
                StartTrigger2 = False
            End If
        End If
    End Sub
    Private Sub DoDeadFrogAnimation()
        GotLady = False
        Dim FrogAnimation = Int((sw.ElapsedMilliseconds - DeadFrogTime) / 550)
        If DeadFrogWater Then
            FrogSprite = FrogWaterDeadSeq(FrogAnimation)
        Else
            FrogSprite = FrogRoadDeadSeq(FrogAnimation)
        End If
        If FrogAnimation = 4 Then
            'round over - restart
            ResetRound()
            FrogLives -= 1
            If FrogLives = 0 Then IsRunning = False
        End If


    End Sub
    Private Sub CheckIfHomePopUpTime()
        Dim popup = Int((sw.ElapsedMilliseconds - HomePopUpTime) / 1000)
        If popup >= 5 Then
            HomePopUpTime = sw.ElapsedMilliseconds
            ClearHomePopUpTime = sw.ElapsedMilliseconds
            'get a random number 0 to 4
            Randomize()
            RandomHome = Int(5 * Rnd())
            Dim PopUpSprite As Integer = 59
            If CurrentRound Mod 2 = 1 Then PopUpSprite = 60
            If Home(RandomHome) = -1 Then Home(RandomHome) = PopUpSprite
        End If
        Dim ClearPopup = Int((sw.ElapsedMilliseconds - HomePopUpTime) / 1000)
        If ClearPopup = 1 And Home(RandomHome) = 60 Then Home(RandomHome) = 61
        If ClearPopup >= 2 Then
            If Home(RandomHome) <> 57 Then Home(RandomHome) = -1
        End If
    End Sub
    Private Sub NewRound()
        'get next round data
        'put new map into the display map
        CurrentRound += 1

        NewRoundFlag = True
        StartTrigger = True
        'ResetRound()
        ET = 0
        PT = 0
        sw.Restart()
        LadyFrogSprite = -2
        GotLady = False
    End Sub
    Private Sub ResetRound()
        TimeLeft = 60
        'TimeLeftScore = 0
        TimeFlag = 0
        StartClock = True
        FrogX = 224
        FrogY = 449
        FrogLane = 12
        frogVelocity = 0
        FrogDead = False
        DeadFrogWater = False
        MaxLane = 12
        'sw.Restart()
        mciSendString("close Timer", CStr(0), 0, 0)
        'mciSendString("play background from 0", CStr(0), 0, 0)
        'Dim Path As String = Chr(34) & "C:\Users\roger\Desktop\Projects\Frogger\Video\background.mp3" & Chr(34)
        ''Dim Path As String = Application.StartupPath
        ''Path = Chr(34) & Path & "\Background.mp3" & Chr(34)
        'Label1.Text = Path
        ''mciSendString("open " & Path & " alias " & "BackGround", Nothing, 0, 0)
        ''mciSendString("setaudio BackGround volume to 100", Nothing, 0, 0)
        ''mciSendString("play BackGround ", Nothing, 0, 0)

        HomePopUpTime = sw.ElapsedMilliseconds
        ClearHomePopUpTime = sw.ElapsedMilliseconds


    End Sub

    Private Sub tickCounter()
        sw.Stop()
        ET = (sw.ElapsedMilliseconds) - PT
        PT += ET
        sw.Start()

        If StartClock Then TimeLeft -= ET / 500
        If TimeLeft < 0 Then TimeLeft = 0
        If HomeTimerStart Then HomeTimer += 1
        If TimeLeftFlag > 0 Then TimeLeftFlag -= ET
        'If ladyFrogFlag > 0 Then ladyFrogFlag -= ET

        If tSec = Int(PT / 1000) And IsRunning = True Then
            frames += 1
        Else
            FPS = frames
            frames = 0
            tSec = Int(PT / 1000)
        End If

    End Sub

    Private Sub GetMaps()
        Dim LineCount As Integer = 0
        Dim Lines = My.Resources.FroggerMaps.Split(CChar(vbCrLf))
        Dim temp As String = ""
        Dim aryTextFile() As String = {"5"}

        Lines(0) = Lines(0).Replace(vbLf, "") 'remove the carriage return
        'get first line = number of maps, rows and columns
        Array.Clear(aryTextFile, 0, aryTextFile.Length)
        aryTextFile = Lines(0).Split(",")

        NumOfMaps = Val(aryTextFile(0))
        NumOfRows = Val(aryTextFile(1))
        NumofSprites = Val(aryTextFile(2))
        ReDim GameMaps(NumOfMaps - 1, NumOfRows - 1, NumofSprites - 1)
        'ReDim CRect(NumofSprites - 1, NumOfRows - 1)
        For MapCount = 0 To NumOfMaps - 1
            'get second line = velocities
            LineCount += 1
            Lines(LineCount) = Lines(LineCount).Replace(vbLf, "") 'remove the carriage return
            Array.Clear(aryTextFile, 0, aryTextFile.Length)
            aryTextFile = Lines(LineCount).Split(",")
            For i = 0 To NumOfRows - 1
                LaneVelocity(MapCount, i) = Val(aryTextFile(i))
            Next
            'get the tile map
            For ii = 0 To NumOfRows - 1
                LineCount += 1
                Lines(LineCount) = Lines(LineCount).Replace(vbLf, "") 'remove the carriage return
                Array.Clear(aryTextFile, 0, aryTextFile.Length)
                aryTextFile = Lines(LineCount).Split(",")
                For i = 0 To NumofSprites - 1
                    GameMaps(MapCount, ii, i) = Val(aryTextFile(i))
                Next
            Next
        Next MapCount
        ConvertMapArray()
        'If Not String.IsNullOrWhiteSpace(lines) Then
        'Console.WriteLine("{0}", LaneVelocity(i))
    End Sub
    Private Sub ConvertMapArray()
        'used to convert the game map for double (smaller) tiles 
        Dim PreviousTile As Integer
        For iii = 0 To NumOfMaps - 1
            For ii = 0 To NumOfRows - 1
                PreviousTile = -1
                For i = 0 To NumofSprites - 1
                    If GameMaps(iii, ii, i) = -1 And PreviousTile > -1 Then GameMaps(iii, ii, i) = -2
                    PreviousTile = GameMaps(iii, ii, i)
                Next
            Next
        Next
        ''print map array in immediate window to check
        'For iii = 0 To NumOfMaps - 1
        '    Console.WriteLine("Map " & iii)
        '    For ii = 0 To NumOfRows - 1
        '        For i = 0 To NumofSprites - 1
        '            Console.Write(GameMaps(iii, ii, i) & ",")
        '        Next
        '        Console.WriteLine()
        '    Next
        'Next

    End Sub

    Private Sub LoadSprites()
        Dim img As New Bitmap(My.Resources.LargeFroggerSheet)
        'get game sprites
        For x As Integer = 0 To 68
            Sprites(x + 0) = New Bitmap(32, 32)
            Using gr As Graphics = Graphics.FromImage(Sprites(x + 0))
                gr.DrawImage(img, 0, 0, New RectangleF((x Mod 8) * 32, Int(x / 8) * 32, 32, 32), GraphicsUnit.Pixel)
            End Using
        Next
        'get timer bars
        For x As Integer = 0 To 7
            TimePics(x) = New Bitmap(16, 16)
            Using gr As Graphics = Graphics.FromImage(TimePics(x))
                gr.DrawImage(img, 0, 0, New RectangleF((x * 16) + 32, 320, 16, 16), GraphicsUnit.Pixel)
            End Using
        Next
        'get status bar pics
        For x As Integer = 0 To 1
            StatusBarPics(x) = New Bitmap(16, 16)
            Using gr As Graphics = Graphics.FromImage(StatusBarPics(x))
                gr.DrawImage(img, 0, 0, New RectangleF(x * 16, 320, 16, 16), GraphicsUnit.Pixel)
            End Using
        Next

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        IsRunning = False
        sw.Stop()
        mciSendString("close BackGround", CStr(0), 0, 0)
        mciSendString("close Timer", CStr(0), 0, 0)
        End
    End Sub

    Private Sub LoadAnimationSequence()
        SinkingTurtleSeq(0) = 44
        SinkingTurtleSeq(1) = 45
        SinkingTurtleSeq(2) = 46
        SinkingTurtleSeq(3) = 47
        SinkingTurtleSeq(4) = 48
        SinkingTurtleSeq(5) = -1
        SinkingTurtleSeq(6) = 48
        SinkingTurtleSeq(7) = 47

        TurtleSeq(0) = 44
        TurtleSeq(1) = 45
        TurtleSeq(2) = 46

        AlligatorSeq(0) = 43
        AlligatorSeq(1) = 42

        FrogRoadDeadSeq(0) = 24
        FrogRoadDeadSeq(1) = 25
        FrogRoadDeadSeq(2) = 26
        FrogRoadDeadSeq(3) = 30

        FrogWaterDeadSeq(0) = 27
        FrogWaterDeadSeq(1) = 28
        FrogWaterDeadSeq(2) = 29
        FrogWaterDeadSeq(3) = 30
    End Sub
    Private Sub ClearHome()
        For i = 0 To 4
            Home(i) = -1
        Next

    End Sub
    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If FrogSprite Mod 2 = 0 Or FrogDead Then Return
        FrogSprite -= 1
    End Sub

    Private Sub ExtractSoundFiles()
        Dim song_path As String
        Dim bytes() As Byte
        'check If file exists
        song_path = Application.StartupPath & "\test.wav"
        'song_path = "Test.wav"
        If Not File.Exists(song_path) Then
            'song_path = My.Computer.FileSystem.GetTempFileName
            ReDim bytes(CInt(My.Resources.Background.Length - 1))
            My.Resources.Background.Read(bytes, 0, CInt(My.Resources.Background.Length))
            'song_path = song_path.Replace(".tmp", ".wav")
            IO.File.WriteAllBytes(song_path, bytes)
        End If
        'song_path = Application.StartupPath & "\Test.wav"
        ''test if extraction was good by playing file
        'Dim _fileToPlay As String = song_path
        'mciSendString("open " & _fileToPlay & " alias myDevice", Nothing, 0, 0)
        'mciSendString("play myDevice", Nothing, 0, 0)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, keyData As System.Windows.Forms.Keys) As Boolean
        If FrogDead Then Return True
        If HomeTimer Then Return True
        Select Case keyData
            Case Keys.Up
                FrogY -= 32
                FrogLane -= 1
                FrogSprite = 1
                frogVelocity = 0
                If FrogLane > 0 And FrogLane < 6 Then
                    frogVelocity = LaneVelocity(CurrentMap, FrogLane)
                End If
                My.Computer.Audio.Play(My.Resources.Jump, AudioPlayMode.Background)
                Return True ' <-- If you want to suppress default handling of arrow keys
            Case Keys.Right
                FrogSprite = 7
                FrogX += 32
                My.Computer.Audio.Play(My.Resources.Jump, AudioPlayMode.Background)
                Return True ' <-- If you want to suppress default handling of arrow keys
            Case Keys.Down
                FrogSprite = 5
                If FrogY < 449 Then
                    FrogY += 32
                    FrogLane += 1
                End If
                frogVelocity = 0
                If FrogLane > 0 And FrogLane < 6 Then
                    frogVelocity = LaneVelocity(CurrentMap, FrogLane)
                End If
                My.Computer.Audio.Play(My.Resources.Jump, AudioPlayMode.Background)
                Return True ' <-- If you want to suppress default handling of arrow keys
            Case Keys.Left
                FrogX -= 32
                FrogSprite = 3
                My.Computer.Audio.Play(My.Resources.Jump, AudioPlayMode.Background)
                Return True ' <-- If you want to suppress default handling of arrow keys
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


End Class
