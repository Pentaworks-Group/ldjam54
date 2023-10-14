using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts
{
    public class JsonEditorBehaviour : JsonEditorBaseBehaviour
    {
        protected override void FillDropdownDataProvider()
        {
            dropDownDataProvider = new Dictionary<string, Func<List<string>>>();
            dropDownDataProvider["Stars"] = GetPossibleStarNames;
            dropDownDataProvider["Spacecrafts"] = GetSpacecraftNames;
            dropDownDataProvider["PlayerSpacecrafts"] = GetSpacecraftNames;
            //dropDownDataProvider["Boolean"] = GetBooleans;
        }

        private List<String> GetBooleans()
        {
            var nameList = new List<String>();
            nameList.Add("True");
            nameList.Add("False");  
            return nameList;

        }


        private List<String> GetPossibleStarNames()
        {
            var nameList = new List<String>();
            foreach (var star in Base.Core.Game.AvailableStars)
            {
                nameList.Add(star.Reference);
            }

            return nameList;
        }


        private List<String> GetSpacecraftNames()
        {
            var nameList = new List<String>();
            foreach (var star in Base.Core.Game.AvailableSpacecrafts)
            {
                nameList.Add(star.Reference);
            }

            return nameList;
        }
    }
}
