﻿/**
 * Outlook integration for SuiteCRM.
 * @package Outlook integration for SuiteCRM
 * @copyright Simon Brooke simon@journeyman.cc
 * @author Simon Brooke simon@journeyman.cc
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU LESSER GENERAL PUBLIC LICENCE as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU LESSER GENERAL PUBLIC LICENCE
 * along with this program; if not, see http://www.gnu.org/licenses
 * or write to the Free Software Foundation,Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301  USA
 */
namespace CrmOutlookAddin.Wrappers
{
    using Core;
    using System;
    using Outlook = Microsoft.Office.Interop.Outlook;

    /// <summary>
    /// A wrapper for an Outlook ContactItem.
    /// </summary>
    public class ContactItem : AbstractItem
    {
        private readonly Outlook.ContactItem item;

        public ContactItem(Outlook.ContactItem item)
        {
            this.item = item;
        }

        /// <summary>
        /// Gets or sets the CRM entry Id.
        /// </summary>
        /// <remarks>
        /// Because Outlook items are not real objects and do not inherit from a common superclass, this
        /// identical code needs to be in each of AbstractAppointmentItem, ContactItem and TaskItem. If
        /// edited in any of those places, please keep the other two in sync.
        /// </remarks>
        public override string CrmEntryId
        {
            get
            {
                string result = null;
                try
                {
                    var prop = item.UserProperties[AbstractItem.CrmIdPropertyName];

                    if (prop != null)
                    {
                        result = prop.Value;
                    }
                }
                catch (Exception) { }

                return result;
            }

            set
            {
                Outlook.UserProperty prop;

                try
                {
                    prop = item.UserProperties[AbstractItem.CrmIdPropertyName];

                    if (prop == null)
                    {
                        prop = item.UserProperties.Add(AbstractItem.CrmIdPropertyName, Outlook.OlUserPropertyType.olText);
                    }
                }
                catch (Exception)
                {
                    prop = item.UserProperties.Add(AbstractItem.CrmIdPropertyName, Outlook.OlUserPropertyType.olText);
                }

                /* don't set it unless the value is actually different. */
                if (prop.Value != value)
                {
                    prop.Value = value;
                }
            }
        }


        public override string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string DistinctFields
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string OutlookId
        {
            get
            {
                return this.item.EntryID;
            }
        }

        /// <summary>
        /// A contact is synchronisable only if its sensitivity is 'normal'.
        /// </summary>
        public override bool Synchronisable
        {
            get
            {
                return SyncDirection.AllowOutbound(Properties.Settings.Default.SyncContacts) && 
                    this.item.Sensitivity == Outlook.OlSensitivity.olNormal;
            }
        }

        public override void CacheItem()
        {
            throw new NotImplementedException();
        }
    }
}
