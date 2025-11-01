using Octokit;

namespace BoTech.SharpStudio.GitServices.Controller;

public class AuthenticationController
{
    public bool Login(string username, string password)
    {
        try
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("BoTech.SharpStudio_v1.0.1.Alpha"));
            client.Credentials = new Credentials(username, password);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }
    public bool IsAuthenticated()
    {
        return true;
    }
}