﻿Public Class Courses
    Dim tab As New Database
    Dim iExit As DialogResult

    'Variables declaration to be assign to text controls
    Dim id, Cname, program, lec, venue, start, endtime As String
    Dim credit, result As Integer

    'close button
    Private Sub btnCose_Click(sender As Object, e As EventArgs) Handles btnCose.Click
        Me.Close()
    End Sub

    'Load lecturer combobox with Data
    Sub loadLectureComboBox()
        tab.ExecuteQuery("SELECT * from lecturer ;")
        tab.HasException()

        LecturerCombo.Items.Clear()
        LecturerCombo.Items.Add("---lecturer---")
        LecturerCombo.SelectedIndex = 0

        For i = 0 To tab.RecordCount - 1
            Dim x As Integer = 1
            LecturerCombo.Items.Add(tab.DatabaseTable(i)(x))
        Next
    End Sub

    'Delete button
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            tab.ExecuteQuery("delete from course where courseID='" & txtCoursecode.Text & "' OR courseName='" & txtCoursename.Text & "';")
            iExit = MessageBox.Show("Record Deleted Successfull", "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            updateTable()
        Catch ex As Exception
            iExit = MessageBox.Show("Record Delete Failed", "Record Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try


    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        If txtCoursecode.Text = "" Or txtCoursename.Text = "" Or Duration.Text = "" Or programmeCombo.SelectedIndex = 0 Or
            VenueComboBox.SelectedIndex = 0 Or LecturerCombo.SelectedIndex = 0 Or txtEnd.Text = "" Or txtEnd.Text = "" Then
            MsgBox("All fields are Required", MsgBoxStyle.Information, "Required fild")

        Else
            id = txtCoursecode.Text
            Cname = txtCoursename.Text

            tab.ExecuteQuery("select * from venue where venueName='" & VenueComboBox.SelectedItem & "';")
            venue = tab.DatabaseTable(0)(0)

            tab.ExecuteQuery("select * from lecturer where fullName='" & LecturerCombo.SelectedItem & "';")
            lec = tab.DatabaseTable(0)(0)


            tab.ExecuteQuery("select * from programme where proName='" & programmeCombo.SelectedItem & "';")
            program = tab.DatabaseTable(0)(0)

            start = txtStart.Text
            endtime = txtEnd.Text
            credit = Duration.Text

            Try
                tab.ExecuteQuery("select * from `course` where `courseID`='" & id & "';")
                result = tab.DatabaseTable.Rows.Count
                If result > 1 Then
                    iExit = MessageBox.Show(Cname & " " & "Already Exist", "Record Exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    tab.ExecuteQuery("Update `course` SET `courseID`='" & id & "', `courseName`='" & Cname & "',
                                        `Duration`= '" & credit & "', `programme`='" & program & "',
                                        `lecID`= '" & lec & "', `venue`='" & venue & "', `start_time`=
                                        '" & start & "', `end_time`='" & endtime & "' where courseID LIKE '" & id & "'
                                        OR courseName LIKE '" & Cname & "';")

                    iExit = MessageBox.Show("Record Updated Successfull", "Record Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    updateTable()
                End If

            Catch ex As Exception
                iExit = MessageBox.Show("Failed to Update Record ", "Insert error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If
    End Sub

    'search button
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            'tab.ExecuteQuery("select * from programme where proName='" & programmeCombo.SelectedItem & "';")
            'program = tab.DatabaseTable(0)(0)

            tab.ExecuteQuery("Select course.courseID As CODE, course.courseName As COURSE, course.duration As CREDIT, programme.proName As PROGRAMME,
                            course.start_time AS START, course.end_time As CLOSE , venue.venueName AS
                            venue From course Join venue On course.venue=venue.venueID Join programme On 
                            programme.proID=course.programme Where courseName ='" & txtsearch.Text & "'
                            OR course.courseID ='" & txtsearch.Text & "' OR course.programme ='" & program & "';")

            result = tab.DatabaseTable.Rows.Count
            If result = 0 Then
                iExit = MessageBox.Show(Cname & " " & "Do not match Any record", "Record Exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                CoursesDataGrid.DataSource = tab.DatabaseTable
                txtsearch.Clear()
            End If

        Catch ex As Exception
            iExit = MessageBox.Show("Failed To Add Record ", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'refresh button
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        updateTable()
    End Sub

    Private Sub CoursesDataGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles CoursesDataGrid.CellClick
        Dim selectedGD As DataGridViewRow
        Dim index As Integer
        index = e.RowIndex
        selectedGD = CoursesDataGrid.Rows(index)

        txtCoursecode.Text = selectedGD.Cells(0).Value.ToString
        txtCoursename.Text = selectedGD.Cells(1).Value.ToString
        Duration.Text = selectedGD.Cells(2).Value.ToString
        programmeCombo.SelectedItem = selectedGD.Cells(3).Value.ToString
        txtStart.Text = selectedGD.Cells(4).Value.ToString
        txtEnd.Text = selectedGD.Cells(5).Value.ToString
        VenueComboBox.SelectedItem = selectedGD.Cells(6).Value.ToString

    End Sub

    'Clear button
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtCoursecode.Clear()
        txtCoursename.Clear()
        txtEnd.Clear()
        Duration.Clear()
        txtStart.Clear()
        LecturerCombo.SelectedIndex = 0
        programmeCombo.SelectedIndex = 0
        VenueComboBox.SelectedIndex = 0

    End Sub


    'Form load
    Private Sub Courses_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        updateTable()
        loadVenueComboBox()
        loadLectureComboBox()
        loadProgrammeComboBox()
    End Sub

    'UpdateTable function
    Sub updateTable()
        tab.ExecuteQuery("Select course.courseID As CODE, course.courseName As COURSE, course.duration AS CREDIT,programme.proName AS PROGRAMME,
                            course.start_time AS START,course.end_time AS CLOSE ,venue.venueName AS
                            VENUE from course JOIN venue ON course.venue=venue.venueID JOIN programme ON programme.proID=course.programme")
        If TAB.HasException(True) Then Exit Sub
        CoursesDataGrid.DataSource = tab.DatabaseTable
    End Sub

    Sub loadLecturerComboBox()
        tab.ExecuteQuery("SELECT * from lecturer ;")
        tab.HasException()

        LecturerCombo.Items.Clear()
        LecturerCombo.Items.Add("---Lecturer---")
        LecturerCombo.SelectedIndex = 0

        For i = 0 To tab.RecordCount - 1
            Dim x As Integer = 1
            LecturerCombo.Items.Add(tab.DatabaseTable(i)(x))
        Next
    End Sub

    Sub loadProgrammeComboBox()
        tab.ExecuteQuery("SELECT * from programme ;")
        tab.HasException()

        programmeCombo.Items.Clear()
        programmeCombo.Items.Add("---programme---")
        programmeCombo.SelectedIndex = 0

        For i = 0 To tab.RecordCount - 1
            Dim x As Integer = 1
            programmeCombo.Items.Add(tab.DatabaseTable(i)(x))
        Next
    End Sub

    Sub loadVenueComboBox()
        tab.ExecuteQuery("SELECT * from venue ;")
        tab.HasException()

        VenueComboBox.Items.Clear()
        VenueComboBox.Items.Add("---Venue---")
        VenueComboBox.SelectedIndex = 0

        For i = 0 To tab.RecordCount - 1
            Dim x As Integer = 1
            VenueComboBox.Items.Add(tab.DatabaseTable(i)(x))
        Next
    End Sub

    'Add records button
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If txtCoursecode.Text = "" Or txtCoursename.Text = "" Or Duration.Text = "" Or programmeCombo.SelectedIndex = 0 Or
            VenueComboBox.SelectedIndex = 0 Or LecturerCombo.SelectedIndex = 0 Or txtEnd.Text = "" Or txtEnd.Text = "" Then
            MsgBox("All fields are Required", MsgBoxStyle.Critical, "Required fild")

        Else
            id = txtCoursecode.Text
            Cname = txtCoursename.Text

            tab.ExecuteQuery("select * from venue where venueName='" & VenueComboBox.SelectedItem & "';")
            venue = tab.DatabaseTable(0)(0)

            tab.ExecuteQuery("select * from lecturer where fullName='" & LecturerCombo.SelectedItem & "';")
            lec = tab.DatabaseTable(0)(0)


            tab.ExecuteQuery("select * from programme where proName='" & programmeCombo.SelectedItem & "';")
            program = tab.DatabaseTable(0)(0)

            start = txtStart.Text
            endtime = txtEnd.Text
            credit = Duration.Text

            Try
                tab.ExecuteQuery("select * from `course` where `courseName`='" & Cname & "';")
                result = tab.DatabaseTable.Rows.Count
                If result > 0 Then 'And id = tab.DatabaseTable(0)(0) Then
                'iExit = MessageBox.Show(" Record ID " & id & " Already Exist", "Record Exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'ElseIf result > 0 And Cname = tab.DatabaseTable(0)(1) Then
                iExit = MessageBox.Show(" Record Name " & Cname & " Already Exist", "Record Exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    tab.ExecuteQuery("INSERT INTO `course` (`courseID`, `courseName`, `Duration`, `programme`, `lecID`, `venue`, `start_time`, `end_time`) 
                        VALUES ('" & id & "', '" & Cname & "', '" & credit & "', '" & program & "', '" & lec & "', '" & venue & "', '" & start & "', '" & endtime & "');")

                    iExit = MessageBox.Show("Record Added Successfull", "Record Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    updateTable()
                End If

            Catch ex As Exception
                iExit = MessageBox.Show("Failed to Add Record ", "Insert error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If

    End Sub

End Class