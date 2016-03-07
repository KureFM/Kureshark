using Kureshark.LocRes;
using System;
using System.Resources;
using System.Windows.Markup;

namespace Kureshark
{
    class LR : MarkupExtension
    {
        #region Fields

        ResourceManager resManager = new ResourceManager(typeof(Language));

        #endregion Fields

        #region Constructors

        public LR()
        {
        }

        public LR(string key)
        {
            Key = key;
        }

        #endregion Constructors

        #region Properties

        [ConstructorArgument("key")]
        public string Key
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public override object ProvideValue(
            IServiceProvider serviceProvider)
        {
            return resManager.GetString(Key);
        }

        #endregion Methods
    }
}
