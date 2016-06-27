using System.IO;
namespace D4Tools.SymbolTableBuilder
{
	public interface IArrayOfSymbol : INamedSymbol
	{
		int Rank { get; }
		string ArrayType { get; }
	}

	// TODO: Come up with a better way to store this data.
	public struct ArrayDimension
	{
		public int StartIndex;
		public int EndIndex;
		public int Range => EndIndex - StartIndex;

		public ArrayDimension(int start, int end)
		{
			StartIndex = start;
			EndIndex = end;
		}
	}

	public class ArrayOfSymbol : IArrayOfSymbol
	{
		public SymbolKind SymbolKind => SymbolKind.ArrayOf;

		readonly ArrayDimension[] dimensions;
		public ArrayDimension[] Dimensions => dimensions;
		public int Rank => dimensions.Length;

		readonly string arrayType;
		public string ArrayType => arrayType;

		public string Name { get; internal set; }
		public ArrayOfSymbol(string name, string arrayType, params ArrayDimension[] dimensions)
		{
			Name = name;

			if (dimensions.Length == 0)
				throw new InvalidDataException($"{nameof(dimensions)} must contain at least one instance.");

			this.dimensions = dimensions;
			this.arrayType = arrayType;
		}
	}
}