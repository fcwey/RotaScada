namespace Entities.Structures
{
    // TIA Portal "Recipe" STRUCT Karşılığı (DB2 Reçete Bloğu)
    public class PlcRecipeStruct
    {
        public short RecipeID { get; set; }            // Int (2 Byte)
        public short RecipeType { get; set; }          // Int (2 Byte)
        public string RecipeName { get; set; } = string.Empty; // String[30] (32 Byte)
        public short TotalStepCount { get; set; }      // Int (2 Byte)

        // Array[1..20] OF "Step" (20 x 42 Byte = 840 Byte)
        public PlcStepStruct[] Steps { get; set; } = new PlcStepStruct[20];

        public PlcRecipeStruct()
        {
            for (int i = 0; i < 20; i++)
            {
                Steps[i] = new PlcStepStruct();
            }
        }
    }
}