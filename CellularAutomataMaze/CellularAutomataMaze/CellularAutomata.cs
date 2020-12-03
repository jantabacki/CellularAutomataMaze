using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellularAutomataMaze
{
    class CellularAutomata
    {
        private readonly ushort[] born;
        private readonly ushort[] survive;
        private bool[,] map;
        private readonly ushort dimX;
        private readonly ushort dimY;
        private bool areCellsAlive = true;

        public bool AreCellsAlive { get => areCellsAlive; set => areCellsAlive = value; }

        public bool[,] Map => map;

        public CellularAutomata(ushort[] born, ushort[] survive, ushort dimX, ushort dimY)
        {
            this.born = born;
            this.survive = survive;
            this.dimX = dimX;
            this.dimY = dimY;
            map = new bool[dimX, dimY];
        }

        public void SeedMap(int seed, int divider)
        {
            Random random = new Random(seed);
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    map[i, j] = bool.Parse((random.Next() % divider) == 0 ? "true" : "false");
                }
            }
        }

        public void NextGeneration()
        {
            bool[,] tempMap = (bool[,])map.Clone();
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    ushort neighboursCount = GetNeighboursCount(i, j);
                    if (born.Any(x => x.Equals(neighboursCount)))
                    {
                        tempMap[i, j] = true;
                    }
                    if (!survive.Any(x => x.Equals(neighboursCount)))
                    {
                        tempMap[i, j] = false;
                    }
                }
            }
            bool foundAliveCells = false;
            for (int i = 0; i < dimX; i++)
            {
                if (foundAliveCells)
                {
                    break;
                }
                for (int j = 0; j < dimY; j++)
                {
                    if (tempMap[i, j] != map[i, j])
                    {
                        foundAliveCells = true;
                        break;
                    }
                }
            }
            map = tempMap;
            areCellsAlive = foundAliveCells;
        }

        private ushort GetNeighboursCount(int x, int y)
        {
            ushort neighbors = 0;
            if (x + 1 < dimX && map[x + 1, y] == true)
            {
                neighbors++;
            }
            if (x - 1 >= 0 && map[x - 1, y] == true)
            {
                neighbors++;
            }
            if (y + 1 < dimY && map[x, y + 1] == true)
            {
                neighbors++;
            }
            if (y - 1 >= 0 && map[x, y - 1] == true)
            {
                neighbors++;
            }
            if (x + 1 < dimX && y + 1 < dimY && map[x + 1, y + 1] == true)
            {
                neighbors++;
            }
            if (x + 1 < dimX && y - 1 >= 0 && map[x + 1, y - 1] == true)
            {
                neighbors++;
            }
            if (x - 1 >= 0 && y + 1 < dimY && map[x - 1, y + 1] == true)
            {
                neighbors++;
            }
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] == true)
            {
                neighbors++;
            }
            return neighbors;
        }
    }
}
