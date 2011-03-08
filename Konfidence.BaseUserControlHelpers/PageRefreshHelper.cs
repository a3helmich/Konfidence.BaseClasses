using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public static class PageRefreshHelper
	{
		public const string CurrentRefreshTicketEntry = "CURRENTREFRESHTICKETENTRY";

		public static bool Check(Page page, HttpContext context, bool asyncPostback)
		{
			bool isRefresh = true;

			int sessionTicket = GetSessionTicket(context, page);

			int pageTicket = GetPageTicket(context, sessionTicket);

			if (pageTicket > sessionTicket || (pageTicket == sessionTicket && pageTicket == 0))
			{
				isRefresh = false;
			}

			if(!asyncPostback)
			{
				UpdateTickets(page, context, pageTicket);
			}

			return isRefresh;
		}

		private static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

		private static int GetSessionTicket(HttpContext context, Page page)
		{
			string currentTicket;

			SessionHelper sessionHelper = new SessionHelper(context, page.UniqueID);
			SessionParameterObject sessionParameterObject = sessionHelper.SessionParameterObject;

			currentTicket = sessionParameterObject.SessionTicket;

			if (!string.IsNullOrEmpty(currentTicket))
			{
				return 0;
			}

			int returnValue;
			if (Int32.TryParse(currentTicket, out returnValue))
			{
				return returnValue;
			}
			return 0;
		}

		private static int GetPageTicket(HttpContext context, int sessionTicket)
		{
			int ticket;

			string currentTicketString = context.Request[CurrentRefreshTicketEntry];

			if (!string.IsNullOrEmpty(currentTicketString))
			{
				ticket = sessionTicket;
			}
			else
			{
				int ticketValue;
				if (Int32.TryParse(currentTicketString, out ticketValue))  // TODO: tryparse and null
				{
					ticket = ticketValue;
				}
				else
				{
					ticket = sessionTicket;
				}
			}

			ticket++;

			return ticket;
		}

		private static void UpdateTickets(Page page, HttpContext context, int assignedTicket)
		{
			string ticket = assignedTicket.ToString(CultureInfo.InvariantCulture);

			page.ClientScript.RegisterHiddenField(CurrentRefreshTicketEntry, ticket);

			SessionHelper sessionHelper = new SessionHelper(context, page.UniqueID);
			SessionParameterObject sessionParameterObject = sessionHelper.SessionParameterObject;

			sessionParameterObject.SessionTicket = ticket;
		}
	}
}