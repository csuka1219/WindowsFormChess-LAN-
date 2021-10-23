using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sakk_Alkalmazás_2._0
{
    public partial class PromotionForm : Form
    {
        public int PromotedPiece;
        public PromotionForm(bool White)
        {
            InitializeComponent();
            if (White)
            {
                btn_BlackBishop.Visible = false;
                btn_BlackKnight.Visible = false;
                btn_BlackQueen.Visible = false;
                btn_BlackRook.Visible = false;
            }
            else
            {
                btn_WhiteBishop.Visible = false;
                btn_WhiteKnight.Visible = false;
                btn_WhiteQueen.Visible = false;
                btn_WhiteRook.Visible = false;
            }
            btn_BlackQueen.Click += Button_click;
            btn_BlackRook.Click += Button_click;
            btn_BlackBishop.Click += Button_click;
            btn_BlackKnight.Click += Button_click;
            btn_WhiteQueen.Click += Button_click;
            btn_WhiteRook.Click += Button_click;
            btn_WhiteBishop.Click += Button_click;
            btn_WhiteKnight.Click += Button_click;
        }
        private void Button_click(object sender, EventArgs e)
        {
            if (sender == btn_BlackQueen)
            {
                PromotedPiece = 05;
            }
            if (sender == btn_BlackRook)
            {
                PromotedPiece = 02;
            }
            if (sender == btn_BlackBishop)
            {
                PromotedPiece = 04;
            }
            if (sender == btn_BlackKnight)
            {
                PromotedPiece = 03;
            }
            if (sender == btn_WhiteQueen)
            {
                PromotedPiece = 15;
            }
            if (sender == btn_WhiteRook)
            {
                PromotedPiece = 12;
            }
            if (sender == btn_WhiteBishop)
            {
                PromotedPiece = 14;
            }
            if (sender == btn_WhiteKnight)
            {
                PromotedPiece = 13;
            }
            this.Close();
        }
    }
}
