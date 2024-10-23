using System;
using UnityEngine;

public partial class Executor
{
    private void HandleAddObject(ScriptCommand command)
    {
        string objectName = command.Arguments[0].Trim('"');
        string sceneName = command.Arguments[2].Trim('"');
        string[] positionParts = command.Arguments[4].Trim('(', ')').Split(',');

        float x = float.Parse(positionParts[0].Split(':')[1]);
        float y = float.Parse(positionParts[1].Split(':')[1]);
        float z = float.Parse(positionParts[2].Split(':')[1]);

        // In Unity, you might use:
        GameObject prefab = Resources.Load<GameObject>(objectName);
        if (prefab != null)
        {
            GameObject instance = GameObject.Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
            Debug.Log($"{objectName} added to scene {sceneName} at position ({x}, {y}, {z})");
        }
        else
        {
            Debug.LogError($"Prefab {objectName} not found.");
        }
    }

    // Handle SET_PROPERTY command.
    private void HandleSetProperty(ScriptCommand command)
    {
        string objectName = command.Arguments[0].Trim('"');
        string componentName = command.Arguments[2].Trim('"');
        string propertyName = command.Arguments[4].Trim('"');
        string propertyValue = command.Arguments[6];

        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            Component component = obj.GetComponent(componentName);
            if (component != null)
            {
                var property = component.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    object convertedValue = Convert.ChangeType(propertyValue, property.PropertyType);
                    property.SetValue(component, convertedValue);
                    Debug.Log($"Set {propertyName} of {objectName}'s {componentName} to {propertyValue}");
                }
                else
                {
                    Debug.LogError($"Property {propertyName} not found in {componentName}");
                }
            }
            else
            {
                Debug.LogError($"Component {componentName} not found in {objectName}");
            }
        }
        else
        {
            Debug.LogError($"Object {objectName} not found.");
        }
    }

    // Handle SET_SKY_TIME command.
    private void HandleSetSkyTime(ScriptCommand command)
    {
        string timeOfDay = command.Arguments[0].Trim('"');
        string timeValue = command.Arguments[2].Trim('"');

        // In Unity, you'd probably control the sky settings via an environment controller.
        Debug.Log($"Sky time set to {timeOfDay} at {timeValue}");
        // Add actual environment-changing code here.
    }
}

