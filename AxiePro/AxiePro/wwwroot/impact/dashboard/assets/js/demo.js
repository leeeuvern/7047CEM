
(function($) {
    'use strict'

    $(function() {
        $('[data-toggle="sweet-alert"]').on('click', function(){
            var type = $(this).data('sweet-alert');

            switch (type) {
                case 'basic':
                    Swal.fire({
                        title: "Here's a message!",
                        text: 'A few words about this sweet alert ...',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-primary'
                    })
                break;

                case 'info':
                    Swal.fire({
                        title: 'Info',
                        text: 'A few words about this sweet alert ...',
                        icon: 'info',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-info'
                    })
                break;

                case 'info':
                    Swal.fire({
                        title: 'Info',
                        text: 'A few words about this sweet alert ...',
                        icon: 'info',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-info'
                    })
                break;

                case 'success':
                    Swal.fire({
                        title: 'Success',
                        text: 'A few words about this sweet alert ...',
                        icon: 'success',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-success'
                    })
                break;

                case 'warning':
                    Swal.fire({
                        title: 'Warning',
                        text: 'A few words about this sweet alert ...',
                        icon: 'warning',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-warning'
                    })
                break;

                case 'question':
                    Swal.fire({
                        title: 'Are you sure?',
                        text: 'A few words about this sweet alert ...',
                        icon: 'question',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-default'
                    })
                break;

                case 'confirm':
                    Swal.fire({
                        title: 'Are you sure?',
                        text: "You won't be able to revert this!",
                        icon: 'warning',
                        showCancelButton: true,
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-danger',
                        confirmButtonText: 'Yes, delete it!',
                        cancelButtonClass: 'btn btn-secondary'
                    }).then((result) => {
                        if (result.value) {
                            // Show confirmation
                            swal({
                                title: 'Deleted!',
                                text: 'Your file has been deleted.',
                                icon: 'success',
                                buttonsStyling: false,
                                confirmButtonClass: 'btn btn-primary'
                            });
                        }
                    })
                break;

                case 'image':
                    Swal.fire({
                        title: 'Sweet',
                        text: "Modal with a custom image ...",
                        imageUrl: '../../assets/img/ill/ill-1.svg',
                        buttonsStyling: false,
                        confirmButtonClass: 'btn btn-primary',
                        confirmButtonText: 'Super!'
                });
                break;

                case 'timer':
                    Swal.fire({
                        title: 'Auto close alert!',
                        text: 'I will close in 2 seconds.',
                        timer: 2000,
                        showConfirmButton: false
                    });
                break;
            }
        });

        if ($('#datetimepicker1').length) {
            $('#datetimepicker1').datetimepicker({
                icons: {
                  time: "fa fa-clock",
                  date: "fa fa-calendar-day",
                  up: "fa fa-chevron-up",
                  down: "fa fa-chevron-down",
                  previous: 'fa fa-chevron-left',
                  next: 'fa fa-chevron-right',
                  today: 'fa fa-screenshot',
                  clear: 'fa fa-trash',
                  close: 'fa fa-remove'
                }
              });
        }

    });
}(jQuery))
