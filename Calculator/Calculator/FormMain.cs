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
            public bool isNumero;
            public bool isDecimalSeparator;
            public bool isPlusMinusSign;
            public ButtonStruct(char content, bool isBold, bool isNumero=false,bool isDecimalSeparator=false, bool isPlusMinusSign=false)
            {
                this.Content = content;
                this.isBold = isBold;
                this.isNumero = isNumero;
                this.isDecimalSeparator = isDecimalSeparator;
                this.isPlusMinusSign = isPlusMinusSign;
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
            {new ButtonStruct('7',true,true), new ButtonStruct('8',true,true), new ButtonStruct('9',true,true), new ButtonStruct('X',false)},
            {new ButtonStruct('4',true,true), new ButtonStruct('5',true,true), new ButtonStruct('6',true,true), new ButtonStruct('-',false)},
            {new ButtonStruct('1',true,true), new ButtonStruct('2',true,true), new ButtonStruct('3',true,true), new ButtonStruct('+',false)},
            {new ButtonStruct('±',false,false,false,true), new ButtonStruct('0',true,true), new ButtonStruct(',',false,false,true), new ButtonStruct('=',false)},
        };
        private RichTextBox resultBox;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GenerateResultBox();
            GenerateButtons(buttons);
        }

        private void GenerateResultBox()
        {
            resultBox = new RichTextBox();
            resultBox.Font = new Font("Segoe UI", 22);
            resultBox.Width = this.Width - 17;
            resultBox.Height = 50;
            resultBox.SelectionAlignment = HorizontalAlignment.Right;
            resultBox.Top = 75;
            resultBox.ReadOnly = true;
            resultBox.TabStop = false;
            resultBox.Text = "0";
            this.Controls.Add(resultBox);
        }

        private void GenerateButtons(ButtonStruct[,] buttons)
        {
            //test 
            int buttonWidth = 80;
            int buttonHeight = 50;
            int posX = 2;
            int posY = 160;
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    Button btn = new Button();
                    btn.Text = buttons[i, j].ToString();
                    btn.Font = new Font("Segoe UI", btn.Font.Size + 6, buttons[i, j].isBold ? FontStyle.Bold : FontStyle.Regular);
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Left = posX + buttonWidth * j;
                    btn.Top = posY + buttonHeight * i;
                    btn.Tag = (ButtonStruct) buttons[i,j];
                    btn.Click += buttonClick;
                    this.Controls.Add(btn);
                }
            }
        }

        private void buttonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ButtonStruct bs = (ButtonStruct)btn.Tag;
            if (bs.isNumero)
            {
                if (resultBox.Text == "0")
                    resultBox.Text = "";
                resultBox.Text += btn.Text;
            }
            else if(bs.isDecimalSeparator)
            {
                if (!resultBox.Text.Contains(bs.Content))
                    resultBox.Text += bs.Content;
            }
            else if(bs.isPlusMinusSign)
            {
                if (!resultBox.Text.Contains("-"))
                    resultBox.Text = $"-{resultBox.Text}";
                else
                    resultBox.Text = resultBox.Text.Replace("-", "");
            }
        }
    }
}
