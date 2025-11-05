using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Animations;
using UnityEditor;

// written by Happyrobot33#2003
// modified by McArdellje (creator of MACS)
namespace MACS
{
    [Serializable]
    public struct ParameterDescription
    {
        public string m_parameter_name;
        public string m_description;
    }

    class ParameterDescriptions : ScriptableObject
    {
        public List<ParameterDescription> m_descriptions = new List<ParameterDescription>();
        static AnimatorController last_controller = null;
        static ParameterDescriptions last_description = null;

        public static ParameterDescriptions GetParameterDescriptions(AnimatorController ctrl, bool create) {
            // return cached description unless it is null and we are creating a new one
            if (ctrl == last_controller && !(last_description == null && create)) {
                return last_description;
            }
            var path = AssetDatabase.GetAssetPath(ctrl);
            ParameterDescriptions desc_obj = AssetDatabase.LoadAssetAtPath<ParameterDescriptions>(path);
            if (desc_obj == null && create) {
                //create the description object
                desc_obj = ScriptableObject.CreateInstance<ParameterDescriptions>();
                desc_obj.name = "MACS_Parameter_Descriptions";
                desc_obj.m_descriptions = new List<ParameterDescription>();
                desc_obj.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
                AssetDatabase.AddObjectToAsset(desc_obj, path);
            } else if (desc_obj != null && desc_obj.hideFlags != (HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild)) {
                // for old versions of MACS that didn't have the hide flags set, set them and save
                desc_obj.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
                EditorUtility.SetDirty(desc_obj);
                AssetDatabase.SaveAssets();
            }
            last_controller = ctrl;
            last_description = desc_obj;
            return desc_obj;
        }

        public static void ParamDescriptionRename(AnimatorController ctrl, string original_name, string new_name)
        {
            // get the description object
            ParameterDescriptions desc_obj = GetParameterDescriptions(ctrl, false);

            // check if the description object exists
            if (desc_obj != null)
            {
                // get index of parameter with original name
                int idx = desc_obj.m_descriptions.FindIndex(
                    (ParameterDescription obj) => obj.m_parameter_name == original_name
                );

                //if it does, rename the entry
                if (idx > 0)
                {
                    // copy the entry
                    ParameterDescription description_entry = desc_obj.m_descriptions[idx];
                    // change the name
                    description_entry.m_parameter_name = new_name;
                    // store the entry
                    desc_obj.m_descriptions[idx] = description_entry;

                    //save the asset
                    EditorUtility.SetDirty(desc_obj);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        public static string GetParamDescription(AnimatorController ctrl, string name)
        {
            // check if the object exists already, and if it does, edit it
            ParameterDescriptions desc_obj = GetParameterDescriptions(ctrl, false);

            if (desc_obj == null)
                return null;

            // get index of parameter
            int idx = desc_obj.m_descriptions.FindIndex(
                (ParameterDescription obj) => obj.m_parameter_name == name
            );

            if (idx < 0)
                return null;
            else
                return desc_obj.m_descriptions[idx].m_description;
        }

        public static void SetParamDescription(AnimatorController ctrl, string name, string description) {
            //check if the object exists already, and if it does, edit it
            ParameterDescriptions desc_obj = GetParameterDescriptions(ctrl, true);

            // if the description object already has an entry for this parameter, edit it
            var idx = desc_obj.m_descriptions.FindIndex(
                (ParameterDescription obj) => obj.m_parameter_name == name
            );

            if (description == null) {
                // delete
                if (idx >= 0) {
                    desc_obj.m_descriptions.RemoveAt(idx);
                }
            } else if (idx >= 0) {
                // already exists, modify
                // copy the entry
                ParameterDescription description_entry = desc_obj.m_descriptions[idx];
                description_entry.m_description = description;

                // store the entry
                desc_obj.m_descriptions[idx] = description_entry;
            } else {
                // create new
                ParameterDescription description_entry = new ParameterDescription();
                description_entry.m_parameter_name = name;
                description_entry.m_description = description;
                desc_obj.m_descriptions.Add(description_entry);
            }
            //save the asset
            EditorUtility.SetDirty(desc_obj);
            AssetDatabase.SaveAssets();
        }
    }
}