using LibGit2Sharp;

namespace BoTech.SharpStudio.GitServices.Controller;

public class CloneController
{
    public void Clone(string sourceUrl, string destinationPath, CloneOptions options)
    {
        //options.FetchOptions.CredentialsProvider = (url, user, types) => new SecureUsernamePasswordCredentials()
        Repository.Clone("https://github.com/libgit2/libgit2sharp.git", "path/to/repo", options);
    }
}