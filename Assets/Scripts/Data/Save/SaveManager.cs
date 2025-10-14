namespace Data.Save
{
    public class SaveManager : ISaveManager
    {
        public void Save()
        {
            PopulateData();
/*            
            Debug.Log("Data saved");
            var savePath = DataManager.DataPath;
            string json = JsonUtility.ToJson(_tempGameData, prettyPrint: true);
            await File.WriteAllTextAsync(savePath, json);
        
#if UNITY_EDITOR
            Debug.Log($"Game saved to: {savePath}\n{json}");
#endif
*/
        }

        private void PopulateData()
        {
            // TODO: Add missing logic
        }
    }
}