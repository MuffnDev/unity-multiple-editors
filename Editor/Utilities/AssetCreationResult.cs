using UnityEngine;

namespace MuffinDev.MultipleEditors.Utilities
{

    /// <summary>
    /// Represents the result of the creation of an asset.
    /// Used by EditorHelpers.CreateAsset() and EditorHelpers.CreateAssetPanel().
    /// </summary>
	public class AssetCreationResult
	{

        #region Properties

        public static readonly AssetCreationResult FAILED_ASSET_CREATION = new AssetCreationResult(false);

        private readonly Object m_CreatedAsset = null;
        private readonly bool m_HasBeenSuccessfullyCreated = false;
        private readonly string m_AbsolutePathToAsset = string.Empty;
        private readonly string m_RelativePathToAsset = string.Empty;

        #endregion


        #region Initialization

        /// <summary>
        /// Default constructor. Used for failedAssetCreation static property.
        /// </summary>
        private AssetCreationResult(bool _FailedAssetCreation) { }

        /// <summary>
        /// Creates an instance of AssetCreationResult.
        /// </summary>
        /// <param name="_AbsolutePath">The path to the folder that will contain the created asset. The path relative to the current
        /// project is deduced from it.</param>
        /// <param name="_Asset">The asset to be created in project's assets.</param>
        public AssetCreationResult(string _AbsolutePath, Object _Asset)
        {
            m_AbsolutePathToAsset = _AbsolutePath;
            m_RelativePathToAsset = EditorHelpers.GetPathRelativeToCurrentProjectFolder(m_AbsolutePathToAsset);

            m_CreatedAsset = _Asset;
            m_HasBeenSuccessfullyCreated = (m_CreatedAsset != null);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Converts this instance into a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format
            (
                "AssetCreationResult: created asset = {0}, relative path = \"{1}\", absolute path = \"{2}\"",
                m_CreatedAsset, m_RelativePathToAsset, m_AbsolutePathToAsset
            );
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Gets the created asset converted to the given type.
        /// </summary>
        /// <returns>Returns the converted created asset, otherwise null.</returns>
        public TAssetType GetAsset<TAssetType>()
            where TAssetType : Object
        {
            return (m_CreatedAsset != null) ? (m_CreatedAsset as TAssetType) : null;
        }

        /// <summary>
        /// Gets the created asset as Object instance.
        /// </summary>
        public Object AssetObject
        {
            get { return m_CreatedAsset; }
        }

        /// <summary>
        /// Gets the absolute path to the created asset.
        /// </summary>
        public string AbsolutePath
        {
            get { return m_AbsolutePathToAsset; }
        }

        /// <summary>
        /// Gets the relative path to the created asset.
        /// </summary>
        public string RelativePath
        {
            get { return m_RelativePathToAsset; }
        }

        /// <summary>
        /// Checks if the asset has successfully been created.
        /// </summary>
        public bool HasBeenSuccessfullyCreated
        {
            get { return m_HasBeenSuccessfullyCreated; }
        }

        #endregion


        #region Operators

        /// <summary>
        /// Returns true if the asset has successfully been created, false if not.
        /// </summary>
        public static implicit operator bool(AssetCreationResult _AssetCreationResult)
        {
            return _AssetCreationResult.HasBeenSuccessfullyCreated;
        }

        #endregion

    }

}