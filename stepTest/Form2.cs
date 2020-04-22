using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stepTest
{
    public partial class Form2 : Form
    {
        double HR85;
        Session currentSession;
        Tested currentTested;
        bool newTest = true;
        DbEntities db;

        public Form2(Session session, DbEntities dbEnt)
        {
            InitializeComponent();
            currentSession = session;
            db = dbEnt;
            currentTested = new Tested();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UpdateTestedListBox();
        }

        private void UpdateTestedListBox()
        {
            // populate left listbox with all test records
            List<HiddenValue> hiddens = new List<HiddenValue>();
            foreach (Tested tested in currentSession.TestedSet.ToList())
            {
                hiddens.Add(new HiddenValue() { Id = tested.Id, Text = tested.name });
            }
            testedListBox.DisplayMember = "Text";
            testedListBox.DataSource = hiddens;
        }

        private void AgeNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // calculate MaxHR and 85% HR
            int age = (int) AgeNumericUpDown1.Value;
            MaxHRTextBox.Text = (220 - age).ToString();
            HR85 = (220 - age) * 0.85;
            HR85textBox.Text = HR85.ToString();
        }

        private void HR1NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((double)HR1NumericUpDown.Value > HR85)
            {
                HR2NumericUpDown.Enabled = false;
                HR2NumericUpDown.Value = 0;
                HR3NumericUpDown.Enabled = false;
                HR3NumericUpDown.Value = 0;
                HR4NumericUpDown.Enabled = false;
                HR4NumericUpDown.Value = 0;
                HR5NumericUpDown.Enabled = false;
                HR5NumericUpDown.Value = 0;
            }
            else
            {
                HR2NumericUpDown.Enabled = true;
                HR3NumericUpDown.Enabled = true;
                HR4NumericUpDown.Enabled = true;
                HR5NumericUpDown.Enabled = true;
            }
        }

        private void HR2NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((double)HR2NumericUpDown.Value > HR85)
            {
                HR3NumericUpDown.Enabled = false;
                HR3NumericUpDown.Value = 0;
                HR4NumericUpDown.Enabled = false;
                HR4NumericUpDown.Value = 0;
                HR5NumericUpDown.Enabled = false;
                HR5NumericUpDown.Value = 0;
            }
            else
            {
                HR3NumericUpDown.Enabled = true;
                HR4NumericUpDown.Enabled = true;
                HR5NumericUpDown.Enabled = true;
            }
        }

        private void HR3NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((double)HR3NumericUpDown.Value > HR85)
            {
                HR4NumericUpDown.Enabled = false;
                HR4NumericUpDown.Value = 0;
                HR5NumericUpDown.Enabled = false;
                HR5NumericUpDown.Value = 0;
            }
            else
            {
                HR4NumericUpDown.Enabled = true;
                HR5NumericUpDown.Enabled = true;
            }
        }

        private void HR4NumericUpDown_ValueChanged_1(object sender, EventArgs e)
        {
                if ((double)HR4NumericUpDown.Value > HR85)
                {
                    HR5NumericUpDown.Enabled = false;
                    HR5NumericUpDown.Value = 0;
                }
                else
                {
                    HR5NumericUpDown.Enabled = true;
                }
        }


        private void checkChecklist(object sender, EventArgs e)
        {
            checkAllForm();
        }
        private void checkAllForm()
        {
            if (readynessCheckBox.Checked && contraIndicationCheckBox.Checked && lifestyleCheckBox.Checked)
            {
                calcButton.Enabled = true;

                HR1NumericUpDown.Enabled = true;
                HR2NumericUpDown.Enabled = true;
                HR3NumericUpDown.Enabled = true;
                HR4NumericUpDown.Enabled = true;
                HR5NumericUpDown.Enabled = true;
            }
            else
            {
                calcButton.Enabled = false;

                HR1NumericUpDown.Enabled = false;
                HR2NumericUpDown.Enabled = false;
                HR3NumericUpDown.Enabled = false;
                HR4NumericUpDown.Enabled = false;
                HR5NumericUpDown.Enabled = false;
            }
        }

        private void testedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get selected db id 
            int testedId = (testedListBox.SelectedItem as HiddenValue).Id;
            currentTested = db.TestedSet.Where(t => t.Id == testedId).FirstOrDefault();

            // clear graph
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            // set all infos in the form
            NameTextBox.Text = currentTested.name;
            AgeNumericUpDown1.Value = currentTested.age;
            readynessCheckBox.Checked = currentTested.readynessToEx;
            contraIndicationCheckBox.Checked = currentTested.contraIndication;
            lifestyleCheckBox.Checked = currentTested.lifestyleActlvl;
            
            MaleRadioButton.Checked = currentTested.sex;
            FemaleRadioButton.Checked = !currentTested.sex;

            aerobicTextBox.Clear();
            fitnessTextBox.Clear();

            HR1NumericUpDown.Value = int.Parse(currentTested.hearthRate.Split(',')[0]);
            HR1NumericUpDown.Enabled = true;
            HR2NumericUpDown.Value = int.Parse(currentTested.hearthRate.Split(',')[1]);
            HR3NumericUpDown.Value = int.Parse(currentTested.hearthRate.Split(',')[2]);
            HR4NumericUpDown.Value = int.Parse(currentTested.hearthRate.Split(',')[3]);
            HR5NumericUpDown.Value = int.Parse(currentTested.hearthRate.Split(',')[4]);
            RemarksRichTextBox.Text = currentTested.remarks;
            // set old record flag
            newTest = false;

            // form validation
            checkAllForm();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear form and re-create all obj
            newTest = true;

            NameTextBox.Clear();
            AgeNumericUpDown1.Value = 0;
            MaxHRTextBox.Clear();
            HR85textBox.Clear();
            MaleRadioButton.Checked = false;
            FemaleRadioButton.Checked = false;
            readynessCheckBox.Checked = false;
            contraIndicationCheckBox.Checked = false;
            lifestyleCheckBox.Checked = false;
            aerobicTextBox.Clear();
            fitnessTextBox.Clear();
            RemarksRichTextBox.Clear();
            HR1NumericUpDown.Value = 0;
            HR2NumericUpDown.Value = 0;
            HR3NumericUpDown.Value = 0;
            HR4NumericUpDown.Value = 0;
            HR5NumericUpDown.Value = 0;

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            checkAllForm();

            currentTested = new Tested();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // delete the record and re-add the updated obj 
            if (!newTest)
            {
                db.TestedSet.Remove(currentTested);
                db.SaveChanges();
            }

            // set obj parameters
            currentTested.name = NameTextBox.Text;
            currentTested.age = (int) AgeNumericUpDown1.Value;
            currentTested.contraIndication = contraIndicationCheckBox.Checked;
            currentTested.lifestyleActlvl = lifestyleCheckBox.Checked;
            currentTested.readynessToEx = readynessCheckBox.Checked;
            currentTested.sex = MaleRadioButton.Checked;
            currentTested.remarks = RemarksRichTextBox.Text;
            currentTested.hearthRate = String.Format("{0},{1},{2},{3},{4}", HR1NumericUpDown.Value, HR2NumericUpDown.Value,
                HR3NumericUpDown.Value, HR4NumericUpDown.Value, HR5NumericUpDown.Value);
            currentTested.SessionSet = currentSession;

            // add to the db
            db.TestedSet.Add(currentTested);
            db.SaveChanges();

            // update left listbox with new infos
            UpdateTestedListBox();
        }

        private void calcButton_Click(object sender, EventArgs e)
        {
            // clear graph
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            
            // define x and y list
            List<int> xCoordinate = new List<int>();
            List<int> yCoordinate = new List<int>();

            // add all HR to the list
            yCoordinate.Add((int) HR1NumericUpDown.Value);
            yCoordinate.Add((int) HR2NumericUpDown.Value);
            yCoordinate.Add((int) HR3NumericUpDown.Value);
            yCoordinate.Add((int) HR4NumericUpDown.Value);
            yCoordinate.Add((int)HR5NumericUpDown.Value);

            // populate x coordinate list with the step height
            switch (currentSession.stepHeight)
            {
                case 15:
                    xCoordinate.Add(11);
                    xCoordinate.Add(14);
                    xCoordinate.Add(18);
                    xCoordinate.Add(21);
                    xCoordinate.Add(25);
                    break;

                case 20:
                    xCoordinate.Add(12);
                    xCoordinate.Add(17);
                    xCoordinate.Add(21);
                    xCoordinate.Add(25);
                    xCoordinate.Add(29);
                    break;

                case 25:
                    xCoordinate.Add(14);
                    xCoordinate.Add(19);
                    xCoordinate.Add(24);
                    xCoordinate.Add(28);
                    xCoordinate.Add(33);
                    break;

                default:
                    xCoordinate.Add(16);
                    xCoordinate.Add(21);
                    xCoordinate.Add(27);
                    xCoordinate.Add(32);
                    xCoordinate.Add(37);
                    break;
            }

            // create list with working values 50%HR < HR < 85%HR
            List<int> yCoordinateWork = new List<int>();
            List<int> xCoordinateWork = new List<int>();
            // calculate HR levels
            double HR50 = double.Parse(MaxHRTextBox.Text) * 0.5;
            double HR85 = double.Parse(HR85textBox.Text);
            double HRMax = double.Parse(MaxHRTextBox.Text);

            for (int i = 0; i < xCoordinate.Count; i++)
            {
                if (yCoordinate[i] != 0)
                {
                    // add point to the graph
                    chart1.Series["HR"].Points.AddXY(xCoordinate[i], yCoordinate[i]);

                    // add points to the work list
                    if (HR50 < yCoordinate[i] && yCoordinate[i] < HR85)
                    {
                        xCoordinateWork.Add(xCoordinate[i]);
                        yCoordinateWork.Add(yCoordinate[i]);
                    }
                }
            }
            // test enough points to calculate the line of best fit
            if (xCoordinateWork.Count >= 2)
            {
                // calculate line of best fit
                // f(x) = slope * x + yIntersept
                int xSum = 0;
                int ySum = 0;
                int xySum = 0;
                int xsquareSum = 0;

                for (int i = 0; i < xCoordinateWork.Count; i++)
                {
                    xSum += xCoordinateWork[i];
                    ySum += yCoordinateWork[i];
                    xySum += (xCoordinateWork[i] * yCoordinateWork[i]);
                    xsquareSum += (int) Math.Pow(xCoordinateWork[i], 2);
                }

                double slope = (xySum - ((xSum * ySum) / xCoordinateWork.Count)) / (xsquareSum - (Math.Pow(xSum, 2) / xCoordinateWork.Count));
                double xbarre = xSum / xCoordinateWork.Count;
                double ybarre = ySum / xCoordinateWork.Count;
                double yIntersept = ybarre - (slope * xbarre);

                double aeroCapacity = (HRMax - yIntersept) / slope;

                // add line of best fit
                chart1.Series["HRBestFit"].Points.AddXY(0, yIntersept);
                chart1.Series["HRBestFit"].Points.AddXY(aeroCapacity, HRMax);

                // add Aerobic line
                chart1.Series["AerobicCapacityLine"].Points.AddXY(aeroCapacity, HRMax);
                chart1.Series["AerobicCapacityLine"].Points.AddXY(aeroCapacity, 0);

                // set results to text box
                aerobicTextBox.Text = aeroCapacity.ToString();
                fitnessTextBox.Text = finesssRating(aeroCapacity, MaleRadioButton.Checked, (int) AgeNumericUpDown1.Value);
                 
            }
            else
            {
                //Console.WriteLine("not enough data");
                MessageBox.Show("You need at least two test HR between 50%MaxHR and 85%MaxHR", "Not enough valid data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private String finesssRating(double aerobicCapacity, bool sex, int age)
        {
            if (sex)
            {
                if (age >= 15 && age <= 19)
                {
                    if (aerobicCapacity < 30)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 38)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 47)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 59)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 20 && age <= 29)
                {
                    if (aerobicCapacity < 28)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 34)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 43)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 54)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 30 && age <= 39)
                {
                    if (aerobicCapacity < 26)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 33)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 39)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 49)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 40 && age <= 49)
                {
                    if (aerobicCapacity < 25)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 31)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 36)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 45)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 50 && age <= 59)
                {
                    if (aerobicCapacity < 23)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 28)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 34)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 43)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 60 && age <= 65)
                {
                    if (aerobicCapacity < 20)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 24)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 32)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 39)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else
                {
                    return "-";
                }
            }
            else
            {
                if (age >= 15 && age <= 19)
                {
                    if (aerobicCapacity < 29)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 35)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 43)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 54)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 20 && age <= 29)
                {
                    if (aerobicCapacity < 27)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 32)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 39)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 49)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 30 && age <= 39)
                {
                    if (aerobicCapacity < 25)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 29)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 35)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 45)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 40 && age <= 49)
                {
                    if (aerobicCapacity < 22)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 27)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 33)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 42)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 50 && age <= 59)
                {
                    if (aerobicCapacity < 21)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 25)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 32)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 40)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else if (age >= 60 && age <= 65)
                {
                    if (aerobicCapacity < 19)
                    {
                        return "Poor";
                    }
                    else if (aerobicCapacity < 23)
                    {
                        return "Below average";
                    }
                    else if (aerobicCapacity < 30)
                    {
                        return "Average";
                    }
                    else if (aerobicCapacity < 38)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Excellent";
                    }
                }
                else
                {
                    return "-";
                }
            }
        }

        private void importFromCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // import csv file

            String fName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            fName = openFileDialog1.FileName;
            StreamReader sr = new StreamReader(fName);
            // read file
            while (sr.Peek() != -1)
            {
                Tested importTested = new Tested();
                String line = sr.ReadLine();
                // set test proprieties
                String[] infoImport = line.Split(',');
                importTested.name = infoImport[0];
                importTested.age = int.Parse(infoImport[1]);
                importTested.sex = (infoImport[2] == "M");
                importTested.lifestyleActlvl = false;
                importTested.readynessToEx = false;
                importTested.contraIndication = false;
                importTested.remarks = "";
                importTested.hearthRate = "0,0,0,0,0";
                importTested.SessionSet = currentSession;
                // add to db
                db.TestedSet.Add(importTested);
            }
            sr.Close();
            db.SaveChanges();

            UpdateTestedListBox();
        }
    }
}
