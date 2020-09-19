using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormMain : Form
    {
        struct ButtonStruct
        {
            public char Content;
            public bool isBold;
            public ButtonStruct(char content, bool isBold)
            {
                this.Content = content;
                this.isBold = isBold;
            }
            public override string ToString()
            {
                return Content.ToString();
            }
        }
        //private char[,] buttons =new char[6,4]; 
        private ButtonStruct[,] buttons =
        {
            {new ButtonStruct('%',false), new ButtonStruct(' ',false), new ButtonStruct('C',false), new ButtonStruct(' ',false)},
            {new ButtonStruct(' ',false), new ButtonStruct(' ',false), new ButtonStruct(' ',false), new ButtonStruct('/',false)},
            {new ButtonStruct('7',true), new ButtonStruct('8',true), new ButtonStruct('9',true), new ButtonStruct('X',false)},
            {new ButtonStruct('4',true), new ButtonStruct('5',true), new ButtonStruct('6',true), new ButtonStruct('-',false)},
            {new ButtonStruct('1',true), new ButtonStruct('2',true), new ButtonStruct('3',true), new ButtonStruct('+',false)},
            {new ButtonStruct('±',false), new ButtonStruct('0',true), new ButtonStruct(',',false), new ButtonStruct('=',false)},
        };
        private RichTextBox resultBox;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GenerateResultBox(resultBox);
            GenerateButtons(buttons);
        }

        private void GenerateResultBox(RichTextBox resultBox)
        {
            resultBox = new RichTextBox();
            resultBox.Font = new Font("Segoe UI", 22);
            resultBox.Width = this.Width - 6;
            resultBox.Height = 50;
            resultBox.Top = 75;
            resultBox.ReadOnly = true;
            this.Controls.Add(resultBox);
        }

        private void GenerateButtons(ButtonStruct[,] buttons)
        {
            int buttonWidth = 80;
            int buttonHeight = 50;
            int posX = 2;
            int posY = 160;
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    Button btn = new Button();
                    //btn.Name = $"btn {buttons[i, j].ToString()}";
                    btn.Text = buttons[i, j].ToString();
                    btn.Font = new Font("Segoe UI", btn.Font.Size + 6, buttons[i, j].isBold ? FontStyle.Bold : FontStyle.Regular);
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Left = posX + buttonWidth * j;
                    btn.Top = posY + buttonHeight * i;
                    this.Controls.Add(btn);
                }
            }
        }
    }
}
