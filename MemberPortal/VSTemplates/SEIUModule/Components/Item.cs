using System;
using System.Web.Caching;

using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;


namespace StagwellTech.SEIU.DNN.Modules.$safeprojectname$.Components
{
    [TableName("SEIU_$safeprojectname$_Items")]
    //setup the primary key for table
    [PrimaryKey("ItemId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Items", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    class Item
    {
        //DATA
        ///<summary>
        /// This is my data item
        ///</summary>
        public string CustomValue { get; set; }
        //DATA END



        //META DATA
        ///<summary>
        /// The ID of your object with the name of the ItemName
        ///</summary>
        public int ItemId { get; set; }
        ///<summary>
        /// The ModuleId of where the object was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who created the object
        ///</summary>
        public int CreatedByUserId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who last updated the object
        ///</summary>
        public int LastModifiedByUserId { get; set; }

        ///<summary>
        /// The date the object was created
        ///</summary>
        public DateTime CreatedOnDate { get; set; }

        ///<summary>
        /// The date the object was updated
        ///</summary>
        public DateTime LastModifiedOnDate { get; set; }
        //META DATA END
    }
}
