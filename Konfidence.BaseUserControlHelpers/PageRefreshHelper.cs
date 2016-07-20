using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public static class PageRefreshHelper 
	{
		public const string CURRENT_REFRESH_TICKET_ENTRY = "CURRENTREFRESHTICKETENTRY";

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

		private static int GetSessionTicket(HttpContext context, Page page)
		{
		    var sessionHelper = new SessionHelper(context, page.UniqueID);

			var sessionParameterObject = sessionHelper.SessionParameterObject;

			var currentTicket = sessionParameterObject.SessionTicket;

			if (currentTicket.IsAssigned())
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

			string currentTicketString = context.Request[CURRENT_REFRESH_TICKET_ENTRY];

			if (currentTicketString.IsAssigned())
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

			page.ClientScript.RegisterHiddenField(CURRENT_REFRESH_TICKET_ENTRY, ticket);

			var sessionHelper = new SessionHelper(context, page.UniqueID);

			var sessionParameterObject = sessionHelper.SessionParameterObject;

			sessionParameterObject.SessionTicket = ticket;
		}
	}
}