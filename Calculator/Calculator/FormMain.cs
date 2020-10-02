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
            public bool isOperator;
            public bool isEqualSign;
            public ButtonStruct(char content, bool isBold, bool isNumero = false, bool isDecimalSeparator = false, bool isPlusMinusSign = false, bool isOperator = false, bool isEqualSign=false)
            {
                this.Content = content;
                this.isBold = isBold;
                this.isNumero = isNumero;
                this.isDecimalSeparator = isDecimalSeparator;
                this.isPlusMinusSign = isPlusMinusSign;
                this.isOperator = isOperator;
                this.isEqualSign = isEqualSign;
            }
            public override string ToString()
            {
                return Content.ToString();
            }
        }
        //private char[,] buttons =new char[6,4]; 
        private ButtonStruct[,] buttons =
        {
            {new ButtonStruct('%',false), new ButtonStruct(' ',false), new ButtonStruct('C',false), new ButtonStruct('<',false)},
            {new ButtonStruct(' ',false), new ButtonStruct(' ',false), new ButtonStruct(' ',false), new ButtonStruct('/',false,false,false,false,true)},
            {new ButtonStruct('7',true,true), new ButtonStruct('8',true,true), new ButtonStruct('9',true,true), new ButtonStruct('X',false,false,false,false,true)},
            {new ButtonStruct('4',true,true), new ButtonStruct('5',true,true), new ButtonStruct('6',true,true), new ButtonStruct('-',false,false,false,false,true)},
            {new ButtonStruct('1',true,true), new ButtonStruct('2',true,true), new ButtonStruct('3',true,true), new ButtonStruct('+',false,false,false,false,true,true)},
            {new ButtonStruct('±',false,false,false,true), new ButtonStruct('0',true,true), new ButtonStruct(',',false,false,true), new ButtonStruct('=',false,false,false,false,true)},
        };
        private const char ASCIIZERO=' ';
        private double operand1, operand2, result;
        private char lastOperator=' ';
        private ButtonStruct lastButtonClicked;
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
            
            resultBox.Width = this.Width - 17;
            resultBox.Height = 50;
            resultBox.Font = new Font("Segoe UI", 22);
            resultBox.SelectionAlignment = HorizontalAlignment.Right;
            resultBox.Top = 75;
            resultBox.ReadOnly = true;
            resultBox.TabStop = false;
            resultBox.TextChanged += ResultBox_TextChanged;
            resultBox.Text = "0";
            this.Controls.Add(resultBox);
        }

        private void ResultBox_TextChanged(object sender, EventArgs e)
        {
            int newSize = 22 + (15 - resultBox.Text.Length);
            if (newSize>10&& newSize<23)
                resultBox.Font = new Font("Segoe UI", newSize);
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
                if(lastButtonClicked.isEqualSign)
                { 
                    clearAll();
                }
                if (resultBox.Text == "0" || lastButtonClicked.isOperator)
                    resultBox.Text = "";
                resultBox.Text += btn.Text;
            }
            else if (bs.isDecimalSeparator)
            {
                if (!resultBox.Text.Contains(bs.Content))
                    resultBox.Text += bs.Content;
            }
            else if (bs.isPlusMinusSign)
            {
                if (resultBox.Text != ("0"))
                {
                    if (!resultBox.Text.Contains("-"))
                        resultBox.Text = $"-{resultBox.Text}";
                    else
                        resultBox.Text = resultBox.Text.Replace("-", "");
                }
            }
            else
            {
                switch (bs.Content)
                {
                    case 'C':
                        clearAll(0);
                        break;
                    case '<':
                        //resultBox.Text = resultBox.Text.Substring(0, resultBox.Text.Length - 1);
                        if (resultBox.Text.Length > 1)
                        {
                            resultBox.Text = resultBox.Text.Remove(resultBox.Text.Length - 1);
                            //if (resultBox.Text.Substring(resultBox.Text.Length - 1) == ",") //rimuovere la virgola se non rimangono numeri a destra di ella
                            //    resultBox.Text = resultBox.Text.Remove(resultBox.Text.Length - 1);
                        }
                        else
                            resultBox.Text = "0";
                        if(resultBox.Text == "-")
                            resultBox.Text = "0";
                        break;
                    default:
                        if(bs.isOperator)
                            manageOperators(bs);
                        break;
                }
            }
            //svolgimento segni e uguale -> if(segnoPrecedente) EseguiSegnoPrecendente() if(segno) AggiungiSegnoSelezionato()
        }

        private void clearAll(double numberToWrite=0)
        {
            operand1 = 0;
            operand2 = 0;
            result = 0;
            lastOperator = ' ';
            resultBox.Text =numberToWrite.ToString();
        }

        private void manageOperators(ButtonStruct bs)
        {
            //presenti vari problemi causati dall'implementazione dell'uguale; rivisitare.
            if (lastOperator == ASCIIZERO)
            {
                operand1 = double.Parse(resultBox.Text);
                lastOperator = bs.Content;
            }
            else
            {
                if (lastButtonClicked.isOperator&&!lastButtonClicked.isEqualSign)
                {
                    lastOperator = bs.Content;
                }
                else
                {
                    if(!lastButtonClicked.isEqualSign)
                            operand2 = double.Parse(resultBox.Text);
                            switch (lastOperator)
                    {
                        case '-':
                            result = operand1 - operand2;
                            break;
                        case '+':
                            result = operand1 + operand2;
                            break;
                        case 'X':
                            result = operand1 * operand2;
                            break;
                        case '/':
                            result = operand1 / operand2;
                            break;
                    }
                    //if(!bs.isEqualSign)
                    lastOperator = bs.Content;
                    operand1 = result;
                    operand2 = 0;
                    resultBox.Text = result.ToString();
                }
            }
            lastButtonClicked = bs;
        }
    }
}
