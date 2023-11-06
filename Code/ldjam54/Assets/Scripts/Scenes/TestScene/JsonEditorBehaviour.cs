using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts
{
    public class JsonEditorBehaviour : JsonEditorBaseBehaviour
    {

        //TODO can we make this static/only once initialised?
        protected override void FillDropdownDataProvider()
        {
            dropDownDataProvider = new Dictionary<string, Func<List<string>>>();
            dropDownDataProvider["Stars"] = GetPossibleStarNames;
            dropDownDataProvider["Spacecrafts"] = GetSpacecraftNames;
            dropDownDataProvider["PlayerSpacecrafts"] = GetSpacecraftNames;
        }

        protected override void FillEditorObjectProvider()
        {
            dropDownEditorProvider = new Dictionary<string, object>();
            dropDownEditorProvider["Stars"] = new Star();
            dropDownEditorProvider["Spacecrafts"] = new Spacecraft();
            dropDownEditorProvider["PlayerSpacecrafts"] = new Spacecraft();
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
