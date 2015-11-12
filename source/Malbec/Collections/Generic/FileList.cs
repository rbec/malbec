using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Malbec.Collections.Generic
{
  public sealed class FileList<T> : IReadOnlyList<T>, IDisposable
    where T : struct
  {
    private readonly MemoryMappedFile File;
    private readonly MemoryMappedViewAccessor View;
    private readonly int HeaderSize;
    private readonly int RecordSize;

    public FileList(string path, int headerSize, int recordSize)
    {
      HeaderSize = headerSize;
      RecordSize = recordSize;
      View = (File = MemoryMappedFile.CreateFromFile(path, FileMode.Open)).CreateViewAccessor();
      Count = (int)((new FileInfo(path).Length - HeaderSize) / RecordSize);
    }

    public IEnumerator<T> GetEnumerator() // TODO: also provide fast stream accessor
    {
      for (var i = 0; i < Count; i++)
        yield return this[i];
    }

    public int Count { get; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T this[int key]
    {
      get
      {
        Lists.CheckBounds(Count, key);
        T value;
        View.Read(key * RecordSize + HeaderSize, out value);
        return value;
      }
    }

    public void Dispose()
    {
      View.Dispose();
      File.Dispose();
    }

    public override string ToString() => this.ToCSV();
  }
}