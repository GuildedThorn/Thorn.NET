CREATE TABLE IF NOT EXISTS Thorn_Users (
    id INT NOT NULL AUTO_INCREMENT,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    discord VARCHAR(18) NOT NULL, -- Discord ID's are 18 characters long (Subject to change)
    email VARCHAR(320) NOT NULL,
    permissions LONG,
    api_key VARCHAR(256) NOT NULL,
    api_keys LONG NOT NULL, -- Array of API keys that they can use in unison with their account api key, these have assigned permissions and are sha-256'ed
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS Kali_Connections (
    id INT NOT NULL AUTO_INCREMENT,
    username INT NOT NULL,
    ip VARCHAR(256) NOT NULL, -- sha256 encrypt this later
    logs LONG NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS Cloud_Connections (
    id INT NOT NULL AUTO_INCREMENT,
    username INT NOT NULL,
    ip VARCHAR(256) NOT NULL, -- sha256 encrypt this later
    logs LONG NOT NULL,
    PRIMARY KEY (id)  
);

CREATE TABLE IF NOT EXISTS API_Connections (
    id INT NOT NULL AUTO_INCREMENT,
    api_key VARCHAR(255) NOT NULL,
    ip VARCHAR(256) NOT NULL, -- sha256 encrypt this later
    data LONG NOT NULL
);