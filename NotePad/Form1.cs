using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NotePad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string seqCount()
        {
            try
            {
                string Connect = "datasource=127.0.0.1;port=3306;username=root;password=ekdnsel;Charset=utf8";
                string Query = "SELECT MAX(noteSeq)+1 AS seqMax FROM dawoon.dc_notemanage;";
                MySqlConnection con = new MySqlConnection(Connect);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(Query, con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    return rdr["seqMax"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(textBoxName.Text == "")
            {
                MessageBox.Show("이름을 입력해주세요");
                textBoxName.Focus();
                return;
            }

            try
            {
                string Connect = "datasource=127.0.0.1;port=3306;username=root;password=ekdnsel;Charset=utf8";
                string Query = "insert into dawoon.dc_notemanage(noteSeq,userNm,memo,flagYN,regDate,issueDate,issueID) values('"
                    + seqCount() + "','" + textBoxName.Text.Trim() + "','" + richTextBoxMemo.Text.Trim() + "','Y',now(),now(),'CDY');";
                MySqlConnection con = new MySqlConnection(Connect);
                MySqlCommand Comm = new MySqlCommand(Query, con);
                MySqlDataReader Read;
                con.Open();
                Read = Comm.ExecuteReader();
                MessageBox.Show("저장완료");
                con.Close();
                textBoxName.Text = "";
                textBoxSearch.Text = "";
                richTextBoxMemo.Text = "";
                buttonSearch_Click(sender, e);
            }
            catch (Exception ex)   
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonSearch_Click(sender, e);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string Connect = "datasource=127.0.0.1;port=3306;username=root;password=ekdnsel;Charset=utf8";
            string Query = "select * from dawoon.dc_notemanage;";
            string searchtext = textBoxSearch.Text.Trim();
            string keyText = comboBoxSearch.Text;
            string field = "";
            if (keyText == "이름") field = "userNm";
            else if (keyText == "메모") field = "memo";
            string flagYN = "";
            if (checkBoxDelShow.Checked == true)
            {
                flagYN = "";
            }
            else
            {
                flagYN = "AND flagYN = 'Y'";
            }
            Query = "select * from dawoon.dc_notemanage WHERE " + field + " like '%" + searchtext + "%' " + flagYN;
            MySqlConnection con = new MySqlConnection(Connect);
            MySqlCommand Comm  = new MySqlCommand(Query, con);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = Comm;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            dataGridView1.DataSource = dTable;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 4].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 3].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 2].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = false;
        }
        private void clear()
        {
            textBoxName.Text = "";
            textBoxSearch.Text = "";
            richTextBoxMemo.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            textBoxName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            richTextBoxMemo.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                string seqstr = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string Connect = "datasource=127.0.0.1;port=3306;username=root;password=ekdnsel;Charset=utf8";
                string Query = "UPDATE dawoon.dc_notemanage set flagYN='N' WHERE noteSeq=" + seqstr;
                MySqlConnection con = new MySqlConnection(Connect);
                MySqlCommand Comm = new MySqlCommand(Query, con);
                MySqlDataReader Read;
                con.Open();
                Read = Comm.ExecuteReader();
                MessageBox.Show("삭제완료");
                con.Close();
                buttonSearch_Click(sender, e);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string seqstr = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string Connect = "datasource=127.0.0.1;port=3306;username=root;password=ekdnsel;Charset=utf8";
                string Query = "UPDATE dawoon.dc_notemanage SET noteSeq='" + seqstr + "',userNm"
                    + textBoxName.Text + "',memo='" + richTextBoxMemo.Text + "; WHERE noteSeq='" + seqstr + "';";
                MySqlConnection con = new MySqlConnection(Connect);
                MySqlCommand Comm = new MySqlCommand(Query, con);
                MySqlDataReader Read;
                con.Open();
                Read = Comm.ExecuteReader();
                MessageBox.Show("수정완료");
                con.Close();
                buttonSave_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "")
            {
                buttonSearch_Click(sender, e);
            }
        }

        private void checkBoxDelShow_CheckedChanged(object sender, EventArgs e)
        {
            buttonSearch_Click(sender, e);
        }
    }
}
