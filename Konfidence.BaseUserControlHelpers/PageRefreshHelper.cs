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
			var isRefresh = true;

			var sessionTicket = GetSessionTicket(context, page);

			var pageTicket = GetPageTicket(context, sessionTicket);

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

		    if (int.TryParse(currentTicket, out var returnValue))
			{
				return returnValue;
			}
			return 0;
		}

		private static int GetPageTicket(HttpContext context, int sessionTicket)
		{
			int ticket;

			var currentTicketString = context.Request[CURRENT_REFRESH_TICKET_ENTRY];

			if (currentTicketString.IsAssigned())
			{
				ticket = sessionTicket;
			}
			else
			{
			    if (int.TryParse(currentTicketString, out var ticketValue))  // TODO: tryparse and null
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
			var ticket = assignedTicket.ToString(CultureInfo.InvariantCulture);

			page.ClientScript.RegisterHiddenField(CURRENT_REFRESH_TICKET_ENTRY, ticket);

			var sessionHelper = new SessionHelper(context, page.UniqueID);

			var sessionParameterObject = sessionHelper.SessionParameterObject;

			sessionParameterObject.SessionTicket = ticket;
		}
	}
}