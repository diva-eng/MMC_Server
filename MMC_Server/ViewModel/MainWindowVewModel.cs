using MMC_Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC_Server.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Global Properties

        private String concertTitle = String.Empty;

        /// <summary>
        /// Gets or sets the concert title.
        /// </summary>
        public String ConcertTitle
        {
            get { return concertTitle; }
            set
            {
                if (value != concertTitle)
                {
                    concertTitle = value;
                    FirePropertyChanged("ConcertTitle");
                    FirePropertyChanged("CanFinalize");
                }
            }
        }
     
        /// <summary>
        /// Gets a value indicating whether the concert can be finalized.
        /// </summary>
        public Boolean CanFinalize
        {
            get { return !String.IsNullOrWhiteSpace(ConcertTitle); }
        }

        #endregion


        #region Character Editor

        private Character selectedCharacter = new Character();

        /// <summary>
        /// Gets or sets currently selected character.
        /// Set null for empty one.
        /// </summary>
        public Character SelectedCharacter
        {
            get { return selectedCharacter; }
            set
            {
                if (selectedCharacter != value)
                {
                    if (value != null)
                        selectedCharacter = value;
                    else
                        selectedCharacter = new Character();

                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }
        

        public String CharacterName
        {
            get { return selectedCharacter.Name; }
            set
            {
                if (value != selectedCharacter.Name)
                {
                    selectedCharacter.Name = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        public String CharacterModelCode
        {
            get { return selectedCharacter.ModelCode; }
            set
            {
                if (value != selectedCharacter.ModelCode)
                {
                    selectedCharacter.ModelCode = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        public String CharacterMotionCode
        {
            get { return selectedCharacter.MotionCode; }
            set
            {
                if (value != selectedCharacter.MotionCode)
                {
                    selectedCharacter.MotionCode = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        public Boolean SpecifyModel
        {
            get { return selectedCharacter.SpecifyModel; }
            set
            {
                if (value != selectedCharacter.SpecifyModel)
                {
                    selectedCharacter.SpecifyModel = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        public Boolean SpecifyMotion
        {
            get { return selectedCharacter.SpecifyMotion; }
            set
            {
                if (value != selectedCharacter.SpecifyMotion)
                {
                    selectedCharacter.SpecifyMotion = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        public Boolean SpecifyVideo
        {
            get { return selectedCharacter.SpecifyVideo; }
            set
            {
                if (value != selectedCharacter.SpecifyVideo)
                {
                    selectedCharacter.SpecifyVideo = value;
                    FirePropertyChanged("SelectedCharacter");
                }
            }
        }

        #endregion

        #region Commands

        #endregion


        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
