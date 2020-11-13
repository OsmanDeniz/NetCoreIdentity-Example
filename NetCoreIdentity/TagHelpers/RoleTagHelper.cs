using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreIdentity.Context;

namespace NetCoreIdentity.TagHelpers
{
    [HtmlTargetElement("showrole")]
    public class RoleTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        public RoleTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public int UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = _userManager.Users.FirstOrDefault(i => i.Id == UserId);
            var roles = await _userManager.GetRolesAsync(user);

            var builder = new StringBuilder();
            foreach (var item in roles)
            {
                builder.Append($"<strong>{item} </strong>");
            }
            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}
