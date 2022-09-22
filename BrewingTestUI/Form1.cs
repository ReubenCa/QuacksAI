using QuacksAI;
using System.Linq;
namespace BrewingTestUI
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            
        }

        List<Token>? StringToTokenList(string a)
        {
            try
            {
                List<string> strtokens =  a.Split('\n').ToList();
                return strtokens.Select<string, Token>(s => StringToToken(s)).ToList();

            }
            catch(Exception e)
            { 
                OutputLabel.Text = e.Message;
                return null;
            }

        }
        
        Token StringToToken(string a)
        {
            string[] b = a.Split(' ');
            object? tokenColorobj;
            if (!Enum.TryParse(typeof(TokenColor), b[0], out tokenColorobj) || tokenColorobj == null)
                throw new Exception("Invalid Token Color " + b[0]);
            return new Token((TokenColor)tokenColorobj, Convert.ToInt32(b[1]));


        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            List<Token>? Board = StringToTokenList(BoardInput.Text);
            List<Token>? Bag = StringToTokenList(BagInput.Text);
            if (Bag == null || Board == null)
                return;
            float RubyWeight = (float)RubyWeightBox.Value;
            float VPWeight = (float)VPWeightBox.Value;
            float MoneyWeight = (float)MoneyWeightBox.Value;
            AIDynamicBrewingParameters d = new AIDynamicBrewingParameters(VPWeight, MoneyWeight, RubyWeight);
            AI ai = new AI(d, new AIStaticBrewingParameters());

            PlayerBrewData PBD = new PlayerBrewData(Bag, Board, (int)StartingTileInput.Value, -1);

            
            OutputLabel.Text = ai.Brew(PBD, out float Score) ? "Should Brew" : "Shouldn't Brew"  ;
            OutputLabel.Text += "\n Score: " + Score.ToString();
        }
    }
}