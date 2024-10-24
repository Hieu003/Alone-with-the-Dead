using UnityEngine;
using UnityEditor;

namespace HQFPSWeapons
{
	public abstract class ProbabilityListDrawer<T> : PropertyDrawer 
	{
		private const float ELEMENT_HEIGHT = 32 + 6;


		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var items = property.FindPropertyRelative("m_Items");
			var probabilities = property.FindPropertyRelative("m_Probabilities");

			// List label
			position.height = 16f;
			property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
			position.y = position.yMax + EditorGUIUtility.standardVerticalSpacing;

			if(property.isExpanded)
			{
				// List size
				position.x += 16;
				position.width -= 16;
				items.arraySize = (int)Mathf.Clamp(EditorGUI.IntField(position, "Size", items.arraySize), 0, 15);

				if(probabilities.arraySize != items.arraySize)
					probabilities.arraySize = items.arraySize;

				position.y = position.yMax + EditorGUIUtility.standardVerticalSpacing;

				for(int i = 0;i < items.arraySize;i ++)
					DrawElement(new Rect(position.x, position.y + i * ELEMENT_HEIGHT, position.width, ELEMENT_HEIGHT), items.GetArrayElementAtIndex(i), probabilities.GetArrayElementAtIndex(i));
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (property.isExpanded ? (property.FindPropertyRelative("m_Items").arraySize * ELEMENT_HEIGHT + 16 + EditorGUIUtility.standardVerticalSpacing * 2) : 0) + 16;
		}

		private void DrawElement(Rect position, SerializedProperty item, SerializedProperty probability)
		{
			GUI.Box(position, "");

			position.height = 16;
			position.y += EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, item, new GUIContent("Element"));

			position.y = position.yMax + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.IntSlider(position, probability, 0, 100, new GUIContent("Probability"));
		}
	}

	[CustomPropertyDrawer(typeof(ItemPickupRandomList))]
	public class ItemPickupRandomListDrawer : ProbabilityListDrawer<ItemPickupRandomList> {  }
}
