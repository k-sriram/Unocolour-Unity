using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public enum CardColor{red, green, yellow, blue, wild, none};

public class CellPos : IEquatable<CellPos> {

	public int x;
	public int y;

	public CellPos(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public CellPos() : this (0,0) {}

	public CellPos(CellPos copy) : this (copy.x,copy.y) {}

	public static CellPos operator + (CellPos a, CellPos b)
	{
		return new CellPos (a.x + b.x, a.y + b.y);
	}
	public static CellPos operator - (CellPos a, CellPos b)
	{
		return new CellPos (a.x - b.x, a.y - b.y);
	}
	public static CellPos operator * (CellPos a, int b)
	{
		return new CellPos (a.x * b, a.y * b);
	}
	public static Vector3 operator * (CellPos a, Vector3 b)
	{
		return new Vector3 (a.x * b.x, a.y * b.y, b.z);
	}

	public static CellPos maxBox (IEnumerable<CellPos> list)
	{
		int maxX = 0;
		int maxY = 0;
		foreach (CellPos i in list) {
			if (i.x > maxX)
				maxX = i.x;
			if (i.y > maxY)
				maxY = i.y;
			}
		return new CellPos (maxX, maxY);

	}

	public int area { get { return x * y; } }


	public void Add(CellPos b)
	{
		x += b.x;
		y += b.y;
	}

	public bool Equals(CellPos other)
	{
		if (other == null) {
			return false;
		}
		return x == other.x && y == other.y;
	}
	public override bool Equals (System.Object obj)
	{
		if (obj == null)
			return false;
		CellPos otherobj = obj as CellPos;
		if (otherobj == null)
			return false;
		else
			return Equals (otherobj);
	}
	public static bool operator == (CellPos a, CellPos b)
	{
		if ((object)a == null || (object)b == null)
			return System.Object.Equals (a, b);
		return a.Equals (b);
	}
	public static bool operator != (CellPos a, CellPos b)
	{
		return !(a == b);
	}

	public override int GetHashCode()
	{
		return x + y * 10;
	}
	public override string ToString ()
	{
		return string.Format ("({0},{1})",x,y);
	}

	public static CellRange Range(CellPos lastCell, bool rowFirst = false, bool invertX = false, bool invertY = false)
	{
		return new CellRange (lastCell, rowFirst, invertX, invertY);
	}

	public CellRange Range(bool rowFirst = false, bool invertX = false, bool invertY = false)
	{
		return new CellRange (this, rowFirst, invertX, invertY);
	}

	public class CellRange : IEnumerable<CellPos>
	{
		int[,] limit;
		int[] ord;
		int[] dir;
		readonly static int[] inc = new int[2] { 1, -1 };
		readonly static int[][] comp = new int[2][ ] { new int [2] { 0, 1 }, new int [2] { 1, 0 } }; 

		public CellRange (CellPos LastCell , bool rowFirst = false, bool invertX = false, bool invertY = false)
		{
			limit = new int [2,2] { { 0, LastCell.x - 1}, { 0, LastCell.y - 1} };
			ord = comp[rowFirst?1:0];
			dir = new int[] {invertX?1:0 ,invertY?1:0};
		}

		public IEnumerator<CellPos> GetEnumerator()
		{
			int[] i = new int[2];
			for (i[0] = limit[ord[0],dir[ord[0]]]; i[0] != limit[ord[0],1-dir[ord[0]]] + inc[dir[ord[0]]]; i[0] += inc[dir[ord[0]]]) {
				for (i[1] = limit[ord[1],dir[ord[1]]]; i[1] != limit[ord[1],1-dir[ord[1]]] + inc[dir[ord[1]]]; i[1] += inc[dir[ord[1]]]) {
					yield return new CellPos (i[ord[0]],i[ord[1]]);
				}
			}
		}



		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator ();
		}
	}
}
	

public class Shape : List<CellPos>
{
	public Shape (IEnumerable<CellPos> collection) : base (collection) {}

	public CellPos max { get { return CellPos.maxBox (this);}}

	public static Shape operator + (Shape a, CellPos b)
	{
		Shape result = new Shape (a);
		for (int i = 0; i < a.Count; i++) {
			result [i] = new CellPos (a[i] + b);
		}
		return result;
	}

	public bool isValidMove(Dictionary<CellPos,CardColor> board)
	{
		List<CardColor> colors = new List<CardColor> (Count);
		for (int i = 0; i < Count; i++) {
			colors.Add( board [this [i]]);
		}
		Dictionary<CardColor,int> total = new Dictionary<CardColor,int>
		{{CardColor.red,0},{CardColor.green,0},{CardColor.blue,0},{CardColor.yellow,0},{CardColor.wild,0}};
		foreach (CardColor i in colors) {
			if (i == CardColor.none)
				return false;
			total [i]++;
		}
		return (total[CardColor.red] <= 1 && total[CardColor.green] <= 1 && total[CardColor.yellow] <= 1 && total[CardColor.blue] <= 1) ||
			total[CardColor.red] + total[CardColor.wild] == 4 || total[CardColor.blue] + total[CardColor.wild] == 4 ||
			total[CardColor.green] + total[CardColor.wild] == 4 || total[CardColor.yellow] + total[CardColor.wild] == 4;
	}

	public override string ToString ()
	{
		return string.Format ("{0} {1} {2} {3}",this[0],this[1],this[2],this[3]);
	}

}


public class BoardEngine 
{
    

	static List<Shape> baseShapes;
	List<Shape> possibleShapes;
	CellPos lastCell;
	Dictionary<Shape,bool> activeShapes;
	Dictionary<CellPos,bool> activeCells;
    List<CellPos> clickedCells;

    public bool CellActive(CellPos pos, bool clickedCellsAreInactive = true)
	{
        if (clickedCellsAreInactive && clickedCells.Contains(pos))
            return false;
        return activeCells[pos];
	}

	static BoardEngine()
	{
		int[][,] baseshapes = new int[19][,]
		{	new int[4,2]{{0,0},{0,1},{1,0},{1,1}},
			new int[4,2]{{0,0},{0,1},{0,2},{1,1}},
			new int[4,2]{{0,0},{1,0},{2,0},{1,1}},
			new int[4,2]{{1,0},{0,1},{1,1},{1,2}},
			new int[4,2]{{1,0},{0,1},{1,1},{2,1}},
			new int[4,2]{{0,0},{1,0},{1,1},{2,1}},
			new int[4,2]{{1,0},{2,0},{0,1},{1,1}},
			new int[4,2]{{1,0},{0,1},{1,1},{0,2}},
			new int[4,2]{{0,0},{0,1},{1,1},{1,2}},
			new int[4,2]{{0,0},{1,0},{1,1},{1,2}},
			new int[4,2]{{2,0},{0,1},{1,1},{2,1}},
			new int[4,2]{{0,0},{0,1},{0,2},{1,2}},
			new int[4,2]{{0,0},{1,0},{2,0},{0,1}},
			new int[4,2]{{0,0},{1,0},{0,1},{0,2}},
			new int[4,2]{{0,0},{1,0},{2,0},{2,1}},
			new int[4,2]{{1,0},{1,1},{0,2},{1,2}},
			new int[4,2]{{0,0},{0,1},{1,1},{2,1}},
			new int[4,2]{{0,0},{0,1},{0,2},{0,3}},
			new int[4,2]{{0,0},{1,0},{2,0},{3,0}}
		};

		baseShapes = new List<Shape> (19);
		foreach (int[,] baseshape in baseshapes) {
			List<CellPos> intbaseshape = new List<CellPos>(4);
			for (int i = 0; i < 4; i++) {
				intbaseshape.Add (new CellPos(baseshape[i,0],baseshape[i,1]));
			}
			baseShapes.Add (new Shape (intbaseshape));
		}
	}

	public BoardEngine(CellPos lastCell)
	{
		this.lastCell = lastCell;
		possibleShapes = new List<Shape> (lastCell.area*baseShapes.Count);

		foreach (Shape baseShape in baseShapes) {
			
			foreach (CellPos shift in (lastCell - baseShape.max).Range()) {
				possibleShapes.Add (baseShape + shift);
			}
		}

		activeShapes = new Dictionary<Shape,bool> (possibleShapes.Count);
		activeCells = new Dictionary<CellPos,bool>(lastCell.area);
        clickedCells = new List<CellPos>(baseShapes[0].Count);
    }



	void UpdateActiveCells()
	{
		foreach (CellPos pos in lastCell.Range()) {
			activeCells [pos] = false;
		}
		foreach (Shape testShape in possibleShapes) {
			if (activeShapes [testShape]) {
				foreach (CellPos pos in testShape) {
					activeCells [pos] = true;
				}
			}
		}
    }
		

	public void RecalculateActiveShapes(Dictionary<CellPos,CardColor> topColors)
	{
        clickedCells.Clear();
        foreach (Shape testShape in possibleShapes) {
			activeShapes [testShape] = testShape.isValidMove (topColors);
		}
		UpdateActiveCells ();
	}

	public void AddCellToMove (CellPos pos)
	{
        clickedCells.Add(pos);
        foreach (Shape testShape in possibleShapes) {
			activeShapes [testShape] = activeShapes [testShape] && testShape.Contains (pos);
		}
		UpdateActiveCells ();
	}

	public bool IsAnyMovePossible ()
	{
		foreach (Shape testShape in possibleShapes) {
			if (activeShapes [testShape])
				return true;
		}
		return false;
	}



}
