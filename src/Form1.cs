namespace TaskB
{
    public partial class Form1 : Form
    {
        Dictionary<string, Student> dictStudent = new Dictionary<string, Student>();
        List<string> enrolCompletedList = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void refresh()
        {
            lBStudentList.DataSource = null;
            lBStudentList.DataSource = dictStudent.Keys.ToList();
            rBCompleted.Checked = true;

            enrolCompletedList.Clear();
            lBEnrolCompleted.DataSource = null;
            foreach (KeyValuePair<string, Student> i in dictStudent)
            {
                if (i.Value.StudentEnrolStatus)
                {
                    enrolCompletedList.Add(i.Key + " :  " + i.Value.StudentName);
                }
            }
            lBEnrolCompleted.DataSource = enrolCompletedList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ID = tBID.Text;
            string Name = tBName.Text;
            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Name) && !dictStudent.ContainsKey(ID))
            {
                Student student = new Student();
                student.studentID = tBID.Text;
                student.StudentName = tBName.Text;
                student.StudentEnrolStatus = rBCompleted.Checked;

                dictStudent[student.studentID] = student;
                refresh();
            }
            else if (dictStudent.ContainsKey(ID))
            {
                dictStudent[ID].StudentEnrolStatus = rBCompleted.Checked;
                tBIDSearch.Clear();
                tBID.Enabled = true;
                tBName.Enabled = true;
            }
            else
            {
                MessageBox.Show("Some fields are empty", "Alert", MessageBoxButtons.OK);
            }
            tBID.Clear();
            tBName.Clear();
            rBCompleted.Checked = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string searchID = tBIDSearch.Text;
            if (!string.IsNullOrEmpty(searchID) && dictStudent[searchID] != null)
            {
                dictStudent.Remove(searchID);
                tBIDSearch.Clear();
                refresh();
            }
            else
            {
                MessageBox.Show("Invalid ID", "Alert", MessageBoxButtons.OK);
            }
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            string searchID = tBIDSearch.Text;
            if (!string.IsNullOrEmpty(searchID) && dictStudent.ContainsKey(searchID))
            {
                MessageBox.Show("Enrolment status :  " + dictStudent[searchID].StudentEnrolStatus, "Alert", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Invalid ID", "Alert", MessageBoxButtons.OK);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string searchID = tBIDSearch.Text;
            if (!string.IsNullOrEmpty(searchID) && dictStudent.ContainsKey(searchID))
            {
                Student student = dictStudent[searchID];
                tBID.Text = student.studentID;
                tBName.Text = student.StudentName;
                rBCompleted.Checked = student.StudentEnrolStatus;
                rBNotCompleted.Checked = !student.StudentEnrolStatus;
                tBID.Enabled = false;
                tBName.Enabled = false;
            }
            else
            {
                MessageBox.Show("Invalid ID", "Alert", MessageBoxButtons.OK);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            string searchName = tBNameSearch.Text;
            string result = "";
            if (dictStudent.Count > 0 && !string.IsNullOrEmpty(searchName))
            {
                foreach (KeyValuePair<string, Student> student in dictStudent)
                {
                    if (student.Value.StudentName == searchName)
                    {
                        result = result + "\n" + student.Key + " :  " + student.Value.StudentEnrolStatus;
                    }
                }
                MessageBox.Show(result, "Alert", MessageBoxButtons.OK);
                tBNameSearch.Clear();
            }
            else
            {
                MessageBox.Show("Name do not exist", "Alert", MessageBoxButtons.OK);
            }
        }
    }

    class Student
    {
        string ID;
        string name;
        Boolean enrol_status;

        public string studentID
        {
            get { return ID; }
            set { ID = value; }
        }

        public string StudentName
        {
            get { return name; }
            set { name = value; }
        }

        public Boolean StudentEnrolStatus
        {
            get { return enrol_status; }
            set { enrol_status = value; }
        }
    }
}