function populateCounty(id, state, county) {
    
    $(id).empty();


    $.ajax({
        url: '../Scripts/countries/county_state.json',
        type: 'GET',
        async: false, //blocks window close
        success: function (data) {
           
            $(id).append($('<option/>', { value: '', text: 'Select...' }));
            $.each(data,
                function (key, subData) {
                  

                    if (key === state) {
                       
                        $.each(subData,
                            function (subkey, subvalue) {
                              

                                $(id)
                                    .append($('<option/>',
                                    {
                                        value: subvalue,
                                        text: subvalue
                                    }));

                            });

                        if (county !== '') {
                            $(id + " option[value='" + county + "']").prop('selected', true);
                        } else {
                            $(id + " option:selected").prop('selected', false);
                        }
                    }
                });
        }
    });
}
function populateStates(id, country, state) {

    $(id).empty();


    $.ajax({
        url: '../Scripts/countries/irsCountries.min.json',
        type: 'GET',
        async: false, //blocks window close
        success: function (data) {
          
            $(id).append($('<option/>', { value: '', text: 'Select...' }));
            $.each(data.country_regions,
                function (key, subData) {

                    if (key === country) {
                      
                        $.each(subData,
                            function (subkey, subvalue) {
                            
                                if (key === 'US') {
                                  
                                    $(id)
                                        .append($('<option/>',
                                        {

                                            value: subkey,
                                            text: subvalue
                                        }));
                                } else {
                                    $(id)
                                        .append($('<option/>',
                                        {
                                            value: subvalue,
                                            text: subvalue
                                        }));
                                }
                            });

                        if (state !== '') {
                            $(id + " option[value='" + state + "']").prop('selected', true);
                        } else {
                            $(id + " option:selected").prop('selected', false);
                        }
                    }
                });
        }
    });
}
function ValidateNumber(e) {
    var charCode = (e.which) ? e.which : e.keyCode
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))
        return false;

    return true;
};
function validateUploadN() {

    var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "gif", "pdf"];
    alert('HI');
    var ext = $("#flUpload").val().split(".");

    if ((ext == null) || (ext == "")) { return true; }
    //$('#Pavalidate').text('Please upload PA ACT file');
    ext = ext[ext.length - 1].toLowerCase();

    if (arrayExtensions.lastIndexOf(ext) == -1) {
        $('#Pavalidate').text('Wrong extension file type');
        $("#flUpload").val("");
        return false;
    }

    return true;

}
function isValid(str) {
    return !/[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}
