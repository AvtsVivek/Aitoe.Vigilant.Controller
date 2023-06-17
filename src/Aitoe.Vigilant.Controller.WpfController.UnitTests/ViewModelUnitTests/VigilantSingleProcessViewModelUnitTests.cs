using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using Ninject;
using Ninject.MockingKernel.FakeItEasy;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using AutoMapper;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.ViewModelUnitTests
{
    public class VigilantSingleProcessViewModelUnitTests
    {

        [Fact]
        public void VigilantSingleProcessViewModelCamRepositoryReturningNoCell()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<VigilantGridViewModel>().ToSelf();
            var camRepoFake = fakingKernel.Get<ICamProcRepository>();
            //A.CallTo(() => camRepoFake.LoadProcInfoFromSettings()).DoesNothing();
            var cells = A.CollectionOfFake<IAitoeRedCell>(0).ToList();
            A.CallTo(() => camRepoFake.GetAllAitoeRedCells()).Returns(cells);
            var vgvm = fakingKernel.Get<VigilantGridViewModel>();
            A.CallTo(() => camRepoFake.LoadProcInfoFromSettings()).MustHaveHappened();
            A.CallTo(() => camRepoFake.GetAllAitoeRedCells()).MustHaveHappened();
            Assert.Equal(0, vgvm.Cells.Count);
        }

        [Fact]
        public void VigilantSingleProcessViewModelCamRepositoryReturningOneCell()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<VigilantGridViewModel>().ToSelf();
            var camRepoFake = fakingKernel.Get<ICamProcRepository>();
            //A.CallTo(() => camRepoFake.LoadProcInfoFromSettings()).DoesNothing();
            var fakeAitoeRedCell = A.Fake<IAitoeRedCell>();

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<IAitoeRedCell, VigilantSingleProcessViewModel>();
            });

            var mapper = mapperConfig.CreateMapper();
            fakingKernel.Bind<IMapper>().ToConstant(mapper);
            
            A.CallTo(() => fakeAitoeRedCell.Column).Returns(1).NumberOfTimes(3);
            A.CallTo(() => fakeAitoeRedCell.Row).Returns(1).NumberOfTimes(3);
            var listOfCells = new List<IAitoeRedCell>(1);
            listOfCells.Add(fakeAitoeRedCell);
            A.CallTo(() => camRepoFake.GetAllAitoeRedCells()).Returns(listOfCells);
            var vgvm = fakingKernel.Get<VigilantGridViewModel>();
            A.CallTo(() => camRepoFake.LoadProcInfoFromSettings()).MustHaveHappened();
            A.CallTo(() => camRepoFake.GetAllAitoeRedCells()).MustHaveHappened();
            Assert.Equal(4, vgvm.Cells.Count);
            var cornerCell = vgvm.Cells.Where(c => c.Row == 0 && c.Column == 0).FirstOrDefault();
            var rowHeaderCell = vgvm.Cells.Where(c => c.Row == 1 && c.Column == 0).FirstOrDefault();
            var columnHeaderCell = vgvm.Cells.Where(c => c.Row == 0 && c.Column == 1).FirstOrDefault();
            var vigilantCell = vgvm.Cells.Where(c => c.Row == 1 && c.Column == 1).FirstOrDefault();

            Assert.NotNull(cornerCell);
            Assert.NotNull(rowHeaderCell);
            Assert.NotNull(columnHeaderCell);
            Assert.NotNull(vigilantCell);
            Assert.IsType(typeof(CornerHeaderCell), cornerCell);
            Assert.IsType(typeof(RowHeaderCell), rowHeaderCell);
            Assert.IsType(typeof(ColumnHeaderCell), columnHeaderCell);
            Assert.IsType(typeof(VigilantSingleProcessViewModel), vigilantCell);
        }
    }
}
