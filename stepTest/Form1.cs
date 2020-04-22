using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stepTest
{
    public partial class Form1 : Form
    {
        DbEntities db = new DbEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void NewSessionButton_Click(object sender, EventArgs e)
        {
            // create new session
            String testerInitials = InitialsTextBox.Text;
            int stepHeight = int.Parse(StepHeightComboBox.SelectedItem.ToString());
            String sessionDate = dateTimePicker.Value.ToString();

            Session newSession = new Session();
            newSession.testerInitials = testerInitials;
            newSession.stepHeight = stepHeight;
            newSession.SessionDate = sessionDate;
            db.SessionSet.Add(newSession);

            /*
            // test db
            Tested newTested = new Tested();
            newTested.name = "maxime";
            newTested.hearthRate = "100,110,120,130,140";
            //newTested.fitnessRating = "fit";
            newTested.age = 20;
            //newTested.aerobicCapacity = 120;
            newTested.contraIndication = false;
            newTested.lifestyleActlvl = true;
            newTested.readynessToEx = true;
            newTested.sex = true;
            newTested.SessionSet = newSession;
            newTested.remarks = "remarks";
            db.TestedSet.Add(newTested);
            */

            db.SaveChanges();

            refreshSessionListBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshSessionListBox();
        }

        private void refreshSessionListBox()
        {
            List<HiddenValue> hiddens = new List<HiddenValue>();
            foreach (Session sess in db.SessionSet.ToList())
            {
                hiddens.Add(new HiddenValue() { Id = sess.Id, Text = sess.testerInitials + "-" + sess.SessionDate });
            }
            SessionListBox.DisplayMember = "Text";

            // prevent the first item to be selected when we set the datasource
            // https://stackoverflow.com/questions/2975635/listbox-and-datasource-prevent-first-item-from-being-selected
            // Get the current selection mode
            SelectionMode selectionMode = SessionListBox.SelectionMode;
            // Set the selection mode to none
            SessionListBox.SelectionMode = SelectionMode.None;
            // Set a new DataSource
            SessionListBox.DataSource = hiddens;
            // Set back the original selection mode
            SessionListBox.SelectionMode = selectionMode;

            /*SessionListBox.Items.Clear();

            foreach (var session in db.SessionSet.ToList())
            {
                SessionListBox.Items.Add(session.Id + "-" +session.testerInitials + "-" + session.SessionDate);
            }*/
        }

        private void SessionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            String itemName = SessionListBox.SelectedItem.ToString();
            int sessionId = int.Parse(itemName.Split('-')[0]);
            */
            int sessionId = (SessionListBox.SelectedItem as HiddenValue).Id;
            Session selectedSession = db.SessionSet.Where(s => s.Id == sessionId).FirstOrDefault();
            Form2 frm =  new Form2(selectedSession, db);
            frm.ShowDialog();
        }
    }
}
