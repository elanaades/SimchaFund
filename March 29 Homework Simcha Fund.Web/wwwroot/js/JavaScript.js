$(() => {
    $("#new-simcha").on('click', function () {
        new bootstrap.Modal($("#add-simcha-modal")[0]).show();
        newSimchaFormValidity();
    })

    $("#new-contributor").on('click', function () {
        new bootstrap.Modal($("#new-contributor-modal")[0]).show();
        newContributorFormValidity();
    })

    $(".deposit-button").on('click', function () {
        const id = $(this).data("contribid");
        $('input[name="contributorId"]').attr('value', id);

        const fullName = $(this).data("full-name");
        $('#deposit-name').text(fullName); 

        new bootstrap.Modal($("#deposit-modal")[0]).show();
        depositFormValidity();
    })

    $(".edit-contrib").on('click', function () {

        $('#initialDepositDiv').hide();

        $('#new-contributor-modal .modal-title').text('Edit Contributor');
        $('#new-contributor-modal .btn-primary').text('Update');

        const firstName = $(this).data("first-name");
        const lastName = $(this).data("last-name")
        const cell = $(this).data("cell")
        const alwaysInclude = $(this).data("always-include")
        const date = $(this).data("date")
        const id = $(this).data("id")

        const form = $('#new-contributor-modal .modal-body');
        form.append(`<input type="hidden" name="Id" value="${id}">`);

        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_cell_number").val(cell);
        $("#contributor_created_at").val(date);
        if (alwaysInclude === "True") {
            $("#contributor_always_include").prop("checked", true);
        }
        else {
            $("#contributor_always_include").prop("checked", false);
        }

        $('form').attr('action', '/contributors/edit');
        new bootstrap.Modal($("#new-contributor-modal")[0]).show();
        editContributorFormValidity();
    })
   
    $("#search").on('keyup', function () {
        const search = $('#search').val().toLowerCase();

        $("table tr:not(:first)").each(function () {
            const name = $(this).find("td:nth-child(2)").text().toLowerCase();
            $(this).toggle(name.includes(search));
        })
    });

    $("#clear").on('click', function () {
        $('#search').val("");
        $("table tr:not(:first)").show();
    });

    const isValidNum = value => !Number.isNaN(Number(value));

    function newContributorFormValidity() {
        $(".btn-primary").prop('disabled', true);

        $('.modal-body').on('input', function () {
            const firstName = $(this).find('#contributor_first_name').val();
            const lastName = $(this).find('#contributor_last_name').val();
            const cell = $(this).find('#contributor_cell_number').val();
            const deposit = $(this).find('input[name="initialDeposit"]').val();
            const date = $(this).find('#contributor_created_at').val();

            const isValid = firstName && lastName && cell && deposit && date && isValidNum(deposit);
            $(".btn-primary").prop('disabled', !isValid);
        })
    }

    function editContributorFormValidity() {
        $(".btn-primary").prop('disabled', false);

        $('.modal-body').on('change', function () {
            const firstName = $(this).find('#contributor_first_name').val();
            const lastName = $(this).find('#contributor_last_name').val();
            const cell = $(this).find('#contributor_cell_number').val();
            const deposit = $(this).find('input[name="initialDeposit"]').val();
            const date = $(this).find('#contributor_created_at').val();

            const isValid = firstName && lastName && cell && deposit && date && isValidNum(deposit);
            $(".btn-primary").prop('disabled', !isValid);
        })
    }

    function newSimchaFormValidity() {
        $('form').on('input', function () {
            const simchaName = $(this).find('input[name="simchaname"]').val();
            const simchaDate = $(this).find('input[name="simchadate"]').val();

            const isValid = simchaName && simchaDate;
            $('button[name="commit"]').prop('disabled', !isValid);
        })
    }

    function depositFormValidity() {
        $('.modal-body').on('input', function () {
            const depositAmount = $(this).find('input[name="depositamount"]').val();
            const depositDate = $(this).find('input[name="depositdate"]').val();
            console.log(depositAmount);
            console.log(depositDate);
            const isValid = depositAmount && depositDate && isValidNum(depositAmount);
            $('.btn-primary').prop('disabled', !isValid);
        })
    }
});