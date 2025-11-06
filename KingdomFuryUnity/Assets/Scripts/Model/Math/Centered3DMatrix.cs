using System;
using Unity.VisualScripting.FullSerializer;

class Centered3DMatrix<T>
{
    private int _radius = 0;
    private int _nCols = 0;
    public int _n = 0;

    private int _center = 0;
    
    private T[] _matrix;
    
    public Centered3DMatrix(int radius)
    {
        _radius = radius;
        _nCols = radius * 2 + 1;
        _n = (int) Math.Pow(_nCols, 3);
        
        _center = (_n - 1) / 2;
        
        _matrix = new T[_n];
    }
    
    public void init(T value)
    {
        int i = 0;
        for (int x = 0; x < _nCols; x++)
        {
            for (int y = 0; y < _nCols; y++)
            {   
                for (int z = 0; z < _nCols; z++)
                {
                    _matrix[To1D(x, y, z)] = value;
                }
            }
            
            i++;
        }
    }

    public override String ToString()
    {
        String s = "";
        int i = 0;

        for (int z = 0; z < _nCols; z++)
        {
            for (int x = _nCols - 1; x >= 0; x--)
            {
                for (int y = 0; y < _nCols; y++)
                {
                    if (_matrix[To1D(x, y, z)] != null)
                    {
                        s += "\t" + _matrix[To1D(x, y, z)].ToString() + " ";
                    }
                    else
                    {
                        s += "\tn";
                    }
                }

                i++;

                s += "\n";
            } 
            
            s += "\n\n";
            
        }
        

        return s;
    }
    
    
    private int To1D(int x, int y, int z)
    {
        return  x * ((int) Math.Pow(_nCols, 2)) + y * _nCols + z;
    } 
    
    // Getters and Setters
    public T this[int x, int y, int z]
    {
        get { return _matrix[To1D(x, y, z) + _center]; }
        set { _matrix[To1D(x, y, z) + _center] = value; }
    }
    
    public T this[int x]
    {
        get { return _matrix[x]; }
        set { _matrix[x] = value; }
    }
    
    public int Radius
    {
        get { return _radius; }
    }
    
    public int Center
    {
        get { return _center; }
    }
    
    public int NCols
    {
        get { return _nCols; }
        set { _nCols = value; }
    }
}
