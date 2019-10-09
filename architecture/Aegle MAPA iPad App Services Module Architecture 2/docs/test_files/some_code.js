Ext.define('com.aegle.mapa.services.SOAApplicationService', {
    constructor: function () {
        return this;
    },

    createSOAApplication: function (application, successCB, errorCB) {
        //Convert the object to JSON
        var app_json_string = Ext.JSON.encode(application);
        //Here encryption_key is a globally available configuration parameter
        var app_encrypted_json_string = CryptoJS.AES.encrypt(app_json_string, encryption_key);
        //Also encrypt the application.complete field
        var is_complete_encrypted = CryptoJS.AES.encrypt(application.complete, encryption_key);
        //Save the application
        da.executeUpdate('insert into soa_app (soa_app_encrypted_json, is_complete_encrypted) values(?,?)',
            [app_encrypted_json_string, is_complete_encrypted],
            function (tx, results) {
                //Set the application id with auto-generated id 
                application.applicationId = results.insertId;
                //Send the created application (with id) back to caller
                successCB(application);
            },
            function (tx, error) {
                //Send the error message back to caller
                errorCB(new PersistenceException(error.message));
            }
        );
    },

    updateSOAApplication: function (application, successCB, errorCB) {
        //Convert the object to JSON
        var app_json_string = Ext.JSON.encode(application);
        //Here encryption_key is a globally available configuration parameter
        var app_encrypted_json_string = CryptoJS.AES.encrypt(app_json_string, encryption_key);
        //Also encrypt the application.complete field
        var is_complete_encrypted = CryptoJS.AES.encrypt(application.complete, encryption_key);
        //Update the application
        da.executeUpdate('update soa_app set soa_app_encrypted_json = ?, is_complete_encrypted = ? where soa_app_id = ?',
            [app_encrypted_json_string, is_complete_encrypted, application.applicationId],
            function (tx, results) {
                //Send the updated application back to caller
                successCB();
            },
            function (tx, error) {
                //Send the error message back to caller
                errorCB(new PersistenceException(error.message));
            }
        );
    },

    deleteSOAApplication: function (applicationId, successCB, errorCB) {
        //Delete the application
        da.executeUpdate('delete from soa_app where soa_app_id = ?', [application.applicationId],
            function (tx, results) {
                //Simply call back to caller
                successCB();
            },
            function (tx, error) {
                //Send the error message back to caller
                errorCB(new PersistenceException(error.message));
            }
        );
    },

    getSOAApplication: function (applicationId, successCB, errorCB) {
        //Get the application
        da.executeQuery('select * from soa_app where soa_app_id = ?', [application.applicationId],
            function (tx, results) {
                var application = null;
                if (results.rows.length == 1) {
                    //Get the encrypted json column from the row
                    var encrypted_json = results.rows.item(0).soa_app_encrypted_json;
                    //Decrypt the json
                    var app_json_string = CryptoJS.AES.decrypt(encrypted_json, encryption_key);
                    //Convert the json back to application instance
                    application = Ext.JSON.decode(app_json_string);
                }
                //Send the retrieved application back to caller
                successCB(application);
            },
            function (tx, error) {
                //Send the error message back to caller
                errorCB(new PersistenceException(error.message));
            }
        );
    },

    cloneSOAApplication: function (applicationId, successCB, errorCB) {
        try {
            //Get the application using the id. This is basically the same implementation as of getSOAApplication

            //Cloning will be done using a {encode object to JSON string -> decode JSON string back to object} cycle
            var cloneApp = Ext.JSON.decode(Ext.JSON.encode(application));

            //Send the cloned application back to caller
            successCB(cloneApp);
        } catch (err) {
            //Send the error back to caller
            errorCB(err);
        }
    },

    searchSOAApplication: function (applicationFilter, successCB, errorCB) {
        //First encrypt the applicationFilter fields so that they can be compared with the respective encrypted fields in DB
        var is_complete_encrypted = CryptoJS.AES.encrypt(applicationFilter.complete, encryption_key);

        //Find the records which match the applicationFilter
        da.executeQuery('select * from soa_app where is_complete_encrypted = ?', [is_complete_encrypted],
            function (tx, results) {
                var matchedApplications = new Array(results.rows.length);
                for (var i = 0; i < results.rows.length; i++ ) {
                    //Get the encrypted json column from the row
                    var encrypted_json = results.rows.item(i).soa_app_encrypted_json;
                    //Decrypt the json
                    var app_json_string = CryptoJS.AES.decrypt(encrypted_json, encryption_key);
                    //Convert the json back to application instance
                    matchedApplications[i] = Ext.JSON.decode(app_json_string);
                }

                //Code here to change further filter the matchedApplications using the applicationFilter's pageSize, pageNumber, sortColumn and sortOrder 

                //Code here to wrap the filtered matchedApplications in a SearchResult JSON object

                //Send the matching applications back to caller
                successCB(matchedApplications);
            },
            function (tx, error) {
                //Send the error message back to caller
                errorCB(new PersistenceException(error.message));
            }
        );
    }
});

