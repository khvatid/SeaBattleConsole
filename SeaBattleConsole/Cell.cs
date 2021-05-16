using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleConsole
{
    internal class Cell
    {
        private bool ship, bombed, fog;

        public Cell() // default contructor with everything false
        {
            ship = false;
            bombed = false;
            fog = false;
        }

        public Cell(bool enemy) // an advanced version of default contructor, with fog creation for enemy cells
        {
            ship = false;
            bombed = false;
            if (enemy)
                fog = true;
        }

        public bool GetShip()
        {
            return ship;
        }

        public bool GetBombed()
        {
            return bombed;
        }

        public void Reveal() // remove the fog, used on adjacent cells when bomb hits ship
        {
            fog = false;
        }

        public bool Bomb() // drop a bomb, return true if hit ship
        {
            Reveal();

            bombed = true;

            return ship;
        }

        public void PlaceShip() // place ship on this cell
        {
            ship = true;
        }

        public int GetStatus() // get cell status
        {
            if (fog)                    // unknown enemy cell
                return 0;
            else if (!ship && !bombed)  // empty unbombed cell
                return 1;
            else if (!ship && bombed)   // empty bombed cell
                return 2;
            else if (ship && !bombed)   // ship unbombed cell
                return 3;
            else                        // ship bombed cell
                return 4;
        }
    }
}