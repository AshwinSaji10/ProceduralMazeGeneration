using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        //creates a maze of specified width and depth
        for(int x = 0; x < _mazeWidth; x++)
        {
            for(int z=0;z< _mazeDepth;z++) 
            { 
                _mazeGrid[x,z]=Instantiate(_mazeCellPrefab,new Vector3(x,0,z),Quaternion.identity);
            
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0,0]);
    }

    private IEnumerator GenerateMaze(MazeCell prevCell,MazeCell curCell) 
    {
        curCell.Visit();
        ClearWalls(prevCell,curCell);

        yield return new WaitForSeconds(0.05f);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(curCell);

            if(nextCell != null)
            {
                yield return  GenerateMaze(curCell, nextCell);
            }

        }while(nextCell != null);   



        if(nextCell != null ) 
        {
            yield return GenerateMaze(curCell,nextCell);
        }


    
    }

    private MazeCell GetNextUnvisitedCell(MazeCell curCell)
    {
        var unvisitedCells=GetUnvisitedCells(curCell);

        return unvisitedCells.OrderBy(_=>Random.Range(1,10)).FirstOrDefault();//randomly choose an unvisited neighbour
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell curCell) 
    {
        int x=(int)curCell.transform.position.x;
        int z = (int)curCell.transform.position.z;

        if(x+1 < _mazeWidth)
        {
            var cellToR = _mazeGrid[x+1,z];
            
            if(cellToR.IsVisited==false)
            {
                yield return cellToR;
            }
            
        }

        if(x-1>=0) 
        {
            var cellToL = _mazeGrid[x-1,z]; 
            if(cellToL.IsVisited==false)
            {
                yield return cellToL;
            }
            
        }
        if(z+1<_mazeDepth) 
        {
            var cellToF = _mazeGrid[x, z + 1];
            if (cellToF.IsVisited == false)
            {
                yield return cellToF;
            }

        }
        if(z-1>=0)
        {
            var cellToB = _mazeGrid[x, z - 1];
            if (cellToB.IsVisited == false)
            {
                yield return cellToB;
            }

        }

    }
    private void ClearWalls(MazeCell prevCell,MazeCell curCell) 
    {
        if(prevCell == null)
        {
            return;
        }

        if(prevCell.transform.position.x< curCell.transform.position.x) 
        {
            prevCell.ClearRightWall();
            curCell.ClearLeftWall();        
            return;
        }

        if (prevCell.transform.position.x > curCell.transform.position.x)
        {
            prevCell.ClearLeftWall();
            curCell.ClearRightWall();           
            return;
        }
        if (prevCell.transform.position.z < curCell.transform.position.z)
        {
            /*prevCell.ClearFrontWall();
            curCell.ClearBackWall();*/

            prevCell.ClearBackWall();
            curCell.ClearFrontWall();
            return;
        }
        if (prevCell.transform.position.z > curCell.transform.position.z)
        {
            prevCell.ClearFrontWall();
            curCell.ClearBackWall();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
