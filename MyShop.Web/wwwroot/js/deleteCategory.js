function DeleteCat(e) {
    var btn = e.currentTarget;  // Get the button that was clicked
    var id = btn.getAttribute('data-cat-id');
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success mx-2",
            cancelButton: "btn btn-danger"
        },
        buttonsStyling: false
    });
    swalWithBootstrapButtons.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Categories/DeleteCategory/' + id,  // Added + for concatenation
                success: function () {
                    swalWithBootstrapButtons.fire({
                        title: "Deleted!",
                        text: "The Category has been deleted.",
                        icon: "success"
                    });
                    $(btn).parents('tr').fadeOut();  // Wrapped btn in jQuery
                },
                error: function () {
                    swalWithBootstrapButtons.fire({
                        title: "Ooops!",
                        text: "Something went wrong.",
                        icon: "error"
                    });
                }
            });
        }
    });
}
