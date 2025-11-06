using System;

class CenteredMatrix<T>
{
    private int _radius = 0;
    private int _nCols = 0;
    private int _n = 0;

    private int _center = 0;
    
    private T[] _matrix;
    
    public CenteredMatrix(int radius)
    {
        _radius = radius;
        _nCols = radius * 2 + 1;
        _n = _nCols * _nCols;
        
        _center = (_n - 1) / 2;
        
        _matrix = new T[_nCols * _nCols];
    }
    
    public void init(T value)
    {
        int i = 0;
        for (int x = 0; x < _nCols; x++)
        {
            for (int y = 0; y < _nCols; y++)
            {
                _matrix[To1D(x, y)] = value;
            }

            i++;
        }
    }

    public override String ToString()
    {
        String s = "";
        int i = 0;
        
        for (int x = _nCols - 1; x >= 0; x--)
        {
            for (int y = 0; y < _nCols; y++)
            {
                if (_matrix[To1D(x, y)] != null)
                {
                    s += "\t" + _matrix[To1D(x, y)].ToString() + " ";
                }
                else
                {
                    s += "\t" + 0 + " ";
                }
            }

            i++;

            s += "\n";
        }

        return s;
    }
    
    private int To2D(int x)
    {
        return  x / _nCols + x % _nCols;
    }
    
    private int To1D(int x, int y)
    {
        return  x * _nCols + y;
    }
    
    // Getters and Setters
    public T this[int x, int y]
    {
        get { return _matrix[To1D(x, y) + _center]; }
        set { _matrix[To1D(x, y) + _center] = value; }
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
