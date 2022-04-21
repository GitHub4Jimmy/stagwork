# Setting up your environment for GitHub Packages

## Setup your settings.xml
    <server>
        <id>maven.pkg.github.com</id>
        <username>GITHUB_USERNAME</username>
        <password>PERSONAL_ACCESS_TOKEN</password>
    </server>

## Add settings to pom.xml
    <distributionManagement>
        <repository>
            <id>maven.pkg.github.com</id>
            <url>https://maven.pkg.github.com/10point1/GITHUB_REPO_NAME</url>
        </repository>
    </distributionManagement>
    ...
    <repository>
        <id>maven.pkg.github.com</id>
        <url>https://maven.pkg.github.com/10point1/GITHUB_REPO_NAME</url>
        <releases><enabled>true</enabled></releases>
        <snapshots><enabled>true</enabled></snapshots>
    </repository>

## Profit
You can now access artifacts on github packages for the repository you have defined.
To publish your own artifacts use the following commands:

    mvn deploy
      OR
    mvn release:prepare
    mvn release:perform
