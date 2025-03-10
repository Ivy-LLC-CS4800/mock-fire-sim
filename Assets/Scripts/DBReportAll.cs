using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System.Text;

public class DatabaseDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public TextMeshProUGUI text7;
    public TextMeshProUGUI text8;

    public TextMeshProUGUI text9;
    public TextMeshProUGUI text10;

    private string connectionString = "URI=file:users.db";

    void Start()
    {
        DisplayTableData();
    }

    void DisplayTableData()
    {
        string[] tableNames = { "users", "simulation", "task", "subtask", "feedback", "environment", "hazard", "tool", "debris", "container" };
        TextMeshProUGUI[] texts = { text1, text2, text3, text4, text5, text6, text7, text8, text9, text10 };

        for (int i = 0; i < tableNames.Length; i++)
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {tableNames[i]}";
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int j = 0; j < reader.FieldCount; j++)
                            {
                                sb.Append(reader.GetName(j) + ": " + reader.GetValue(j).ToString() + "\n");
                            }
                            sb.Append("\n");
                        }
                    }
                }
            }
            texts[i].text = sb.ToString();
        }
    }
}
