global using identityServerWeb.Configuration;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Mvc.RazorPages;

global using System.Text;
global using System.Text.Json;
global using System.Security.Claims;
global using System.Diagnostics.Metrics;
global using System.ComponentModel.DataAnnotations;

global using Duende.IdentityModel;
global using Duende.IdentityServer;
global using Duende.IdentityServer.Test;
global using Duende.IdentityServer.Models;
global using Duende.IdentityServer.Stores;
global using Duende.IdentityServer.Events;
global using Duende.IdentityServer.Services;
global using Duende.IdentityServer.Validation;
global using Duende.IdentityServer.Extensions;