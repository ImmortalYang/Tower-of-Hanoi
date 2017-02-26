using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Towers_of_Hanoi
{
    public class Disk
    {
        private int diameter;
        private int color;
        private int level; // Position in each peg
        private int pNum; // Number of peg 
        private Label label;

        /// <summary>
        /// default constructor
        /// </summary>
        public Disk()
        {
            diameter = 0;
            color = 0;
            level = 0;
            pNum = 0;
            label = null;
        }

        /// <summary>
        /// Alternative constructor
        /// public Disk(int diam, int col, int lv, int peg)
        /// </summary>
        /// <param name="aLabel">the label correspond to the disk</param>
        /// <param name="col">color of the disk</param>
        /// <param name="lv">level of the disk in the peg</param>
        /// <param name="peg">the peg of the disk</param>
        public Disk(Label aLabel, int col, int lv, int peg)
        {
            diameter = aLabel.Width;
            color = col;
            level = lv;
            pNum = peg;
            label = aLabel;
        }

        /// <summary>
        /// This method set new level in Disk object.
        /// </summary>
        /// <param name="lvl"></param>
        public void setLevel(int lvl)
        {
            level = lvl;
        }

        /// <summary>
        /// This mehod get level stored in disk object.
        /// </summary>
        /// <returns></returns>
        public int getLevel()
        {
            return level;
        }

        /// <summary>
        /// This method set color of Disk object.
        /// </summary>
        /// <param name="col"></param>
        public void setColor(int col)
        {
            color = col;
        }

        /// <summary>
        /// This method get color stored in Disk object. 
        /// </summary>
        /// <returns></returns>
        public int getColor()
        {
            return color;
        }

        /// <summary>
        /// This method set diameter in Disk object.
        /// @param Label is passed.
        /// </summary>
        /// <param name="aLabel"></param>
        public void setDiameter(Label aLabel)
        {
            diameter = aLabel.Width;
        }

        /// <summary>
        /// This method get diameter of Disk object.
        /// @return width of associated label.  
        /// </summary>
        /// <returns></returns>
        public int getDiameter()
        {
            return diameter;
        }

        /// <summary>
        /// This method stores peg index in Disk object.
        /// @param disk index is passed. 
        /// </summary>
        /// <param name="pn"></param>
        public void setPegNum(int pn)
        {
            pNum = pn;
        }

        /// <summary>
        /// This method get the index of the peg stored in Disk object.
        /// @return index of peg. 
        /// </summary>
        /// <returns></returns>
        public int getPegNum()
        {
            return pNum;
        }

        /// <summary>
        /// This method get label stored in Disk object.    
        /// </summary>
        /// <returns></returns>
        public Label getLabel()
        {
            return label;
        }

        /// <summary>
        /// This object set label in disk object.
        /// @param label
        /// </summary>
        /// <param name="aLabel"></param>
        public void setLabel(Label aLabel)
        {
            label = aLabel;
        }

        /// <summary>
        /// Move the disk up in the animation
        /// by changing the coordinate of the correspond label
        /// </summary>
        public void moveUp()
        {
            this.getLabel().Location =
                new Point(this.getLabel().Location.X, this.getLabel().Location.Y - 2);
        }

        /// <summary>
        /// Move the disk down in the animation
        /// by changing the coordinate of the correspond label
        /// </summary>
        public void moveDown()
        {
            this.getLabel().Location =
                new Point(this.getLabel().Location.X, this.getLabel().Location.Y + 2);
        }

        /// <summary>
        /// Move the disk left in the animation
        /// by changing the coordinate of the correspond label
        /// </summary>
        public void moveLeft()
        {
            this.getLabel().Location =
                new Point(this.getLabel().Location.X - 2, this.getLabel().Location.Y);
        }

        /// <summary>
        /// Move the disk right in the animation
        /// by changing the coordinate of the correspond label
        /// </summary>
        public void moveRight()
        {
            this.getLabel().Location =
                new Point(this.getLabel().Location.X + 2, this.getLabel().Location.Y);
        }
    }
}
